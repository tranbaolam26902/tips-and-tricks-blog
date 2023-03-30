using System.Net;
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
                .Produces<ApiResponse<PaginationResult<Comment>>>();

            routeGroupBuilder.MapPost("/{id:int}", ChangeCommentApprovalStatus)
                .WithName("ChangeCommentApprovalStatus")
                .Produces(204)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteComment)
                .WithName("DeleteComment")
                .Produces(204)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetComments([AsParameters] CommentQuery query, [AsParameters] PagingModel pagingModel, ICommentRepository commentRepository) {
            var comments = await commentRepository.GetPagedCommentsByQueryAsync(query, pagingModel);
            var paginationResult = new PaginationResult<Comment>(comments);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> ChangeCommentApprovalStatus(int id, ICommentRepository commentRepository) {
            return await commentRepository.ChangeCommentApprovedState(id)
                ? Results.Ok(ApiResponse.Success("Đổi trạng thái thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bình luận có mã số {id}"));
        }

        private static async Task<IResult> DeleteComment(int id, ICommentRepository commentRepository) {
            return await commentRepository.DeleteCommentByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Xóa thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bình luận có mã số {id}"));
        }
    }
}
