using Mapster;
using MapsterMapper;
using TipsAndTricks.Core.Collections;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.WebApi.Models;

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

            return app;
        }

        public static async Task<IResult> GetPosts([AsParameters] PostQuery query, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), query, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(paginationResult);
        }

        public static async Task<IResult> GetPopularPosts(int limit, IBlogRepository blogRepository, IMapper mapper) {
            var posts = await blogRepository.GetPopularArticlesAsync(limit);

            return posts.Count != 0 ? Results.Ok(mapper.Map<IList<PostDTO>>(posts)) : Results.NotFound("Không có bài viết");
        }

        public static async Task<IResult> GetRandomPosts(int limit, IBlogRepository blogRepository, IMapper mapper) {
            var posts = await blogRepository.GetRandomPostsAsync(limit);

            return posts.Count != 0 ? Results.Ok(mapper.Map<IList<PostDTO>>(posts)) : Results.NotFound("Không có bài viết");
        }

        public static async Task<IResult> GetArchivePosts(int limit, IBlogRepository blogRepository, IMapper mapper) {
            var posts = await blogRepository.CountMonthlyPostsAsync(limit);

            return posts.Count != 0 ? Results.Ok(mapper.Map<IList<MonthlyPostCountItem>>(posts)) : Results.NotFound("Không có bài viết");
        }

        public static async Task<IResult> GetPostById(int id, IBlogRepository blogRepository, IMapper mapper) {
            var post = await blogRepository.GetPostByIdAsync(id);

            return post != null ? Results.Ok(mapper.Map<PostDetail>(post)) : Results.NotFound($"Không tìm thấy bài viết có mã số {id}");
        }
    }
}
