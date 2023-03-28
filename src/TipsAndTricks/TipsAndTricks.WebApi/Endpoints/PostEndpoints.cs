using Mapster;
using MapsterMapper;
using TipsAndTricks.Core.Collections;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.Services.Media;
using TipsAndTricks.WebApi.Filter;
using TipsAndTricks.WebApi.Models;
using TipsAndTricks.WebApi.Models.Posts;

namespace TipsAndTricks.WebApi.Endpoints {
    public static class PostEndpoints {
        public static WebApplication MapPostEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/posts");

            routeGroupBuilder.MapGet("/", GetPosts)
                .WithName("GetPosts")
                .Produces<PaginationResult<PostDTO>>();
            routeGroupBuilder.MapGet("/featured/{limit:int}", GetPopularPosts)
                .WithName("GetPopularPosts")
                .Produces<IList<PostDTO>>();
            routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
                .WithName("GetRandomPosts")
                .Produces<IList<PostDTO>>();
            routeGroupBuilder.MapGet("/archive/{limit:int}", GetArchivePosts)
                .WithName("GetArchivePosts")
                .Produces<IList<MonthlyPostCountItem>>();
            routeGroupBuilder.MapGet("/{id:int}", GetPostById)
                .WithName("GetPostById")
                .Produces<PostDetail>()
                .Produces(404);
            routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9_-]+$)}", GetPostBySlug)
                .WithName("GetPostBySlug")
                .Produces<PostDetail>()
                .Produces(404);
            routeGroupBuilder.MapPost("/", AddPost)
                .WithName("AddPost")
                .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                .Produces(201)
                .Produces(400)
                .Produces(409);
            routeGroupBuilder.MapPost("/{id:int}/thumbnail", SetPostImage)
                .WithName("SetPostImage")
                .Accepts<IFormFile>("multipart/form-data")
                .Produces<string>()
                .Produces(404);
            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                .WithName("DeletePost")
                .Produces(204)
                .Produces(404);
            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
                .WithName("UpdatePost")
                .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                .Produces(204)
                .Produces(400)
                .Produces(409);
            routeGroupBuilder.MapGet("/{id:int}/comments", GetPostComments)
                .WithName("GetPostComments")
                .Produces<IList<Comment>>();

            return app;
        }

        private static async Task<IResult> GetPosts([AsParameters] PostQuery query, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), query, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetPopularPosts(int limit, IBlogRepository blogRepository, IMapper mapper) {
            var posts = await blogRepository.GetPopularArticlesAsync(limit);

            return posts.Count != 0 ? Results.Ok(mapper.Map<IList<PostDTO>>(posts)) : Results.NotFound("Không có bài viết");
        }

        private static async Task<IResult> GetRandomPosts(int limit, IBlogRepository blogRepository, IMapper mapper) {
            var posts = await blogRepository.GetRandomPostsAsync(limit);

            return posts.Count != 0 ? Results.Ok(mapper.Map<IList<PostDTO>>(posts)) : Results.NotFound("Không có bài viết");
        }

        private static async Task<IResult> GetArchivePosts(int limit, IBlogRepository blogRepository, IMapper mapper) {
            var posts = await blogRepository.CountMonthlyPostsAsync(limit);

            return posts.Count != 0 ? Results.Ok(mapper.Map<IList<MonthlyPostCountItem>>(posts)) : Results.NotFound("Không có bài viết");
        }

        private static async Task<IResult> GetPostById(int id, IBlogRepository blogRepository, IMapper mapper) {
            var post = await blogRepository.GetPostByIdAsync(id);

            return post != null ? Results.Ok(mapper.Map<PostDetail>(post)) : Results.NotFound($"Không tìm thấy bài viết có mã số {id}");
        }

        private static async Task<IResult> GetPostBySlug(string slug, IBlogRepository blogRepository, IMapper mapper) {
            var post = await blogRepository.GetPostBySlugAsync(slug);

            return post != null ? Results.Ok(mapper.Map<PostDetail>(post)) : Results.NotFound($"Không tìm thấy bài viết có slug {slug}");
        }

        private static async Task<IResult> AddPost(PostEditModel model, IAuthorRepository authorRepository, IBlogRepository blogRepository, IMapper mapper) {
            if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug)) {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var post = mapper.Map<Post>(model);
            post.PostedDate = DateTime.Now;
            await blogRepository.EditPostAsync(post, model.SelectedTags);

            return Results.CreatedAtRoute("GetPostById", new { post.Id }, mapper.Map<PostDetail>(post));
        }

        private static async Task<IResult> SetPostImage(int id, IFormFile imageFile, IBlogRepository blogRepository, IMediaManager mediaManager) {
            var imageUrl = await mediaManager.SaveFileAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl)) {
                return Results.BadRequest("Không lưu được tập tin");
            }

            await blogRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(imageUrl);
        }

        private static async Task<IResult> DeletePost(int id, IBlogRepository blogRepository) {
            return await blogRepository.DeletePostById(id)
                ? Results.NoContent()
                : Results.NotFound($"Không thể tìm thấy bài viết có mã số {id}");
        }

        private static async Task<IResult> UpdatePost(int id, PostEditModel model, IBlogRepository blogRepository, IAuthorRepository authorRepository, IMapper mapper) {
            var post = await blogRepository.GetPostByIdAsync(id);

            if (post == null)
                return Results.NotFound($"Không tìm thấy bài viết có mã số '{id}");

            if (await blogRepository.IsPostSlugExistedAsync(id, model.UrlSlug)) {
                return Results.Conflict(
                    $"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var isExitsCategory = await blogRepository.GetCategoryByIdAsync(model.CategoryId);
            var isExitsAuthor = await authorRepository.GetAuthorByIdAsync(model.AuthorId);

            if (isExitsAuthor == null || isExitsCategory == null) {
                return Results.NotFound($"Mã tác giả hoặc chủ đề không tồn tại!");
            }

            mapper.Map(model, post);
            post.Id = id;
            post.Category = null;
            post.Author = null;

            return await blogRepository.EditPostAsync(post, model.SelectedTags) != null
                ? Results.NoContent()
                : Results.NotFound();
        }

        private static async Task<IResult> GetPostComments(int id, ICommentRepository commentRepository) {
            var comments = await commentRepository.GetPostCommentsAsync(id);

            return Results.Ok(comments);
        }
    }
}
