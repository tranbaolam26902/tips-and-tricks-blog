using System.Net;
using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.WebApi.Models;
using TipsAndTricks.WebApi.Models.Subscribers;

namespace TipsAndTricks.WebApi.Endpoints {
    public static class SubscribeEnpoints {
        public static WebApplication MapSubscribeEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/subscribers");

            routeGroupBuilder.MapGet("/", GetSubscribers)
                .WithName("GetSubscribers")
                .Produces<ApiResponse<IPagedList<Subscriber>>>();

            routeGroupBuilder.MapPut("/{id:int}/ban", BanSubscriber)
                .WithName("BanSubscriber")
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}/delete", DeleteSubscriber)
                .WithName("DeleteSubscriber")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPost("/{email:regex(^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{{2,3}}$)}/subscribe", Subscribe)
                .WithName("Subscribe")
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPost("/{email:regex(^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{{2,3}}$)}/unsubscribe", Unsubscribe)
                .WithName("Unsubscribe")
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetSubscribers([AsParameters] SubscriberQuery query, [AsParameters] PagingModel pagingModel, ISubscriberRepository subscriberRepository) {
            var subscribers = await subscriberRepository.GetPagedSubscribersByQueryAsync(query, pagingModel);


            return Results.Ok(ApiResponse.Success(subscribers));
        }

        private static async Task<IResult> BanSubscriber(int id, SubscriberEditModel model, ISubscriberRepository subscriberRepository) {
            if (model.IsBanned) {
                return await subscriberRepository.BanSubscriberAsync(id, model.Notes)
                    ? Results.Ok(ApiResponse.Success("Chặn thành công", HttpStatusCode.NoContent))
                    : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy người đăng ký với id = {id}"));
            } else {
                return await subscriberRepository.UnbanSubscriberAsync(id)
                    ? Results.Ok(ApiResponse.Success("Bỏ chặn thành công", HttpStatusCode.NoContent))
                    : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy người đăng ký với id = {id}"));
            }
        }

        private static async Task<IResult> DeleteSubscriber(int id, ISubscriberRepository subscriberRepository) {
            return await subscriberRepository.DeleteSubscriberByIdAsync(id)
                    ? Results.Ok(ApiResponse.Success("Xóa thành công", HttpStatusCode.NoContent))
                    : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy người đăng ký với id = {id}"));
        }

        private static async Task<IResult> Subscribe(string email, ISubscriberRepository subscriberRepository) {
            var isSuccess = await subscriberRepository.SubscribeAsync(email);

            if (!isSuccess) {
                var subscriber = await subscriberRepository.GetSubscriberByEmailAsync(email);
                if (subscriber.SubscribeState == SubscribeState.Banned)
                    return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Email {email} này đã bị cấm bởi quản trị viên!"));
                if (subscriber.SubscribeState == SubscribeState.Subscribe) {
                    return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Email {email} đã đăng ký nhận thông báo trước đó!"));
                }
            }

            return Results.Ok(ApiResponse.Success(HttpStatusCode.NoContent));
        }

        private static async Task<IResult> Unsubscribe(string email, UnsubscribeEditModel model, ISubscriberRepository subscriberRepository) {
            return await subscriberRepository.UnsubscribeAsync(email, model.Reason)
                    ? Results.Ok(ApiResponse.Success("Hủy đăng ký thành công", HttpStatusCode.NoContent))
                    : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy người đăng ký với email = {email}"));
        }
    }
}
