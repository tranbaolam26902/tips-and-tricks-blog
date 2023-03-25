using TipsAndTricks.Core.Collections;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.WebApi.Models;

namespace TipsAndTricks.WebApi.Endpoints {
    public static class CommentEndpoints {
        public static WebApplication MapCommentEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/comments");

            routeGroupBuilder.MapGet("/", GetComments)
                .WithName("GetComments")
                .Produces<PaginationResult<Comment>>();

            return app;
        }

        private static async Task<IResult> GetComments([AsParameters] CommentQuery query, [AsParameters] PagingModel pagingModel, ICommentRepository commentRepository) {
            var comments = await commentRepository.GetPagedCommentsByQueryAsync(query, pagingModel);
            var paginationResult = new PaginationResult<Comment>(comments);

            return Results.Ok(paginationResult);
        }
    }
}
