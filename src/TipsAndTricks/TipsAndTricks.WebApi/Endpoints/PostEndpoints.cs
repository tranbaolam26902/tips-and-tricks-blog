using Mapster;
using MapsterMapper;
using System.Net;
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
                .Produces<ApiResponse<PaginationResult<PostDTO>>>();

            routeGroupBuilder.MapGet("/featured/{limit:int}", GetPopularPosts)
                .WithName("GetPopularPosts")
                .Produces<ApiResponse<IList<PostDTO>>>();

            routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
                .WithName("GetRandomPosts")
                .Produces<ApiResponse<IList<PostDTO>>>();

            routeGroupBuilder.MapGet("/archive/{limit:int}", GetArchivePosts)
                .WithName("GetArchivePosts")
                .Produces<ApiResponse<IList<MonthlyPostCountItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetPostById)
                .WithName("GetPostById")
                .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9_-]+$)}", GetPostBySlug)
                .WithName("GetPostBySlug")
                .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapPost("/", AddPost)
                .WithName("AddPost")
                .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                .Produces(201)
                .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapPost("/{id:int}/thumbnail", SetPostImage)
                .WithName("SetPostImage")
                .Accepts<IFormFile>("multipart/form-data")
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                .WithName("DeletePost")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
                .WithName("UpdatePost")
                .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapGet("/{id:int}/comments", GetPostComments)
                .WithName("GetPostComments")
                .Produces<ApiResponse<IList<Comment>>>();

            return app;
        }

        private static async Task<IResult> GetPosts([AsParameters] PostQuery query, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), query, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPopularPosts(int limit, IBlogRepository blogRepository, IMapper mapper) {
            var posts = await blogRepository.GetPopularArticlesAsync(limit);

            return posts.Count != 0
                ? Results.Ok(ApiResponse.Success(mapper.Map<IList<PostDTO>>(posts)))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không có bài viết"));
        }

        private static async Task<IResult> GetRandomPosts(int limit, IBlogRepository blogRepository, IMapper mapper) {
            var posts = await blogRepository.GetRandomPostsAsync(limit);

            return posts.Count != 0
                ? Results.Ok(ApiResponse.Success(mapper.Map<IList<PostDTO>>(posts)))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không có bài viết"));
        }

        private static async Task<IResult> GetArchivePosts(int limit, IBlogRepository blogRepository, IMapper mapper) {
            var posts = await blogRepository.CountMonthlyPostsAsync(limit);

            return posts.Count != 0
                ? Results.Ok(ApiResponse.Success(mapper.Map<IList<MonthlyPostCountItem>>(posts)))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không có bài viết"));
        }

        private static async Task<IResult> GetPostById(int id, IBlogRepository blogRepository, IMapper mapper) {
            var post = await blogRepository.GetPostByIdAsync(id);

            return post != null
                ? Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}"));
        }

        private static async Task<IResult> GetPostBySlug(string slug, IBlogRepository blogRepository, IMapper mapper) {
            var post = await blogRepository.GetPostBySlugAsync(slug);
            if (post != null)
                await blogRepository.IncreaseViewCountAsync(post.Id);

            return post != null
                ? Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có slug {slug}"));
        }

        private static async Task<IResult> AddPost(PostEditModel model, IAuthorRepository authorRepository, IBlogRepository blogRepository, IMapper mapper) {
            if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug)) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var post = mapper.Map<Post>(model);
            post.PostedDate = DateTime.Now;
            await blogRepository.EditPostAsync(post, model.SelectedTags);

            return Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post), HttpStatusCode.Created));
        }

        private static async Task<IResult> SetPostImage(int id, IFormFile imageFile, IBlogRepository blogRepository, IMediaManager mediaManager) {
            var imageUrl = await mediaManager.SaveFileAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl)) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await blogRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        private static async Task<IResult> DeletePost(int id, IBlogRepository blogRepository) {
            return await blogRepository.DeletePostById(id)
                ? Results.Ok(ApiResponse.Success("Xóa thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy tác giả có mã số {id}"));
        }

        private static async Task<IResult> UpdatePost(int id, PostEditModel model, IBlogRepository blogRepository, IAuthorRepository authorRepository, IMapper mapper) {
            var post = await blogRepository.GetPostByIdAsync(id);

            if (post == null)
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy bài viết có mã số {id}"));

            if (await blogRepository.IsPostSlugExistedAsync(id, model.UrlSlug)) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var isExitsCategory = await blogRepository.GetCategoryByIdAsync(model.CategoryId);
            var isExitsAuthor = await authorRepository.GetAuthorByIdAsync(model.AuthorId);

            if (isExitsAuthor == null || isExitsCategory == null) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Mã tác giả hoặc chủ đề không tồn tại"));
            }

            mapper.Map(model, post);
            post.Id = id;
            post.Category = null;
            post.Author = null;

            return await blogRepository.EditPostAsync(post, model.SelectedTags) != null
                ? Results.Ok(ApiResponse.Success("Cập nhật thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy tác giả có mã số {id}"));
        }

        private static async Task<IResult> GetPostComments(int id, ICommentRepository commentRepository) {
            var comments = await commentRepository.GetPostCommentsAsync(id);

            return Results.Ok(ApiResponse.Success(comments));
        }
    }
}
