using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using TipsAndTricks.Core.Collections;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
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

        private static async Task PopulatePostEditModelAsync(PostEditModel model, IAuthorRepository authorRepository, IBlogRepository blogRepository) {
            var authors = await authorRepository.GetAuthorsAsync();
            var categories = await blogRepository.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem() {
                Text = a.FullName,
                Value = a.Id.ToString()
            });

            model.CategoryList = categories.Select(c => new SelectListItem() {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        private static async Task<IResult> AddPost(PostEditModel model, IAuthorRepository authorRepository, IBlogRepository blogRepository, IMapper mapper) {
            await PopulatePostEditModelAsync(model, authorRepository, blogRepository);

            if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug)) {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var post = mapper.Map<Post>(model);
            await blogRepository.EditPostAsync(post, model.GetSelectedTags());

            return Results.CreatedAtRoute("GetPostById", new { post.Id }, mapper.Map<PostDetail>(post));
        }
    }
}
