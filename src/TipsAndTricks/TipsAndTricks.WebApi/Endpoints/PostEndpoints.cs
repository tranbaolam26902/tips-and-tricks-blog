using Mapster;
using TipsAndTricks.Core.Collections;
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

            return app;
        }

        public static async Task<IResult> GetPosts([AsParameters] PostQuery query, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), query, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(paginationResult);
        }
    }
}
