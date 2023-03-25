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
                .Produces<IPagedList<Subscriber>>();
            routeGroupBuilder.MapPut("/{id:int}/ban", BanSubscriber)
                .WithName("BanSubscriber")
                .Produces(404);
            routeGroupBuilder.MapDelete("/{id:int}/unban", DeleteSubscriber)
                .WithName("DeleteSubscriber")
                .Produces(204)
                .Produces(404);
            routeGroupBuilder.MapPost("/{email:regex(^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{{2,3}}$)}/subscribe", Subscribe)
                .WithName("Subscribe")
                .Produces(204)
                .ProducesProblem(400);
            routeGroupBuilder.MapPost("/{email:regex(^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{{2,3}}$)}/unsubscribe", Unsubscribe)
                .WithName("Unsubscribe")
                .Produces(204)
                .Produces(404);

            return app;
        }

        private static async Task<IResult> GetSubscribers([AsParameters] SubscriberQuery query, [AsParameters] PagingModel pagingModel, ISubscriberRepository subscriberRepository) {
            var subscribers = await subscriberRepository.GetPagedSubscribersByQueryAsync(query, pagingModel);


            return Results.Ok(subscribers);
        }

        private static async Task<IResult> BanSubscriber(int id, SubscriberEditModel model, ISubscriberRepository subscriberRepository) {
            if (model.IsBanned) {
                return await subscriberRepository.BanSubscriberAsync(id, model.Notes)
                    ? Results.NoContent()
                    : Results.NotFound($"Không tìm thấy người theo dõi có mã số {id}");
            } else {
                return await subscriberRepository.UnbanSubscriberAsync(id)
                    ? Results.NoContent()
                    : Results.NotFound($"Không tìm thấy người theo dõi có mã số {id}");
            }
        }

        private static async Task<IResult> DeleteSubscriber(int id, ISubscriberRepository subscriberRepository) {
            return await subscriberRepository.DeleteSubscriberByIdAsync(id)
                ? Results.NoContent()
                : Results.NotFound($"Không tìm thấy người theo dõi có mã số {id}");
        }

        private static async Task<IResult> Subscribe(string email, ISubscriberRepository subscriberRepository) {
            var isSuccess = await subscriberRepository.SubscribeAsync(email);

            if (!isSuccess) {
                var subscriber = await subscriberRepository.GetSubscriberByEmailAsync(email);
                if (subscriber.SubscribeState == SubscribeState.Banned)
                    return Results.Problem($"Email {email} này đã bị cấm bởi quản trị viên!");
                if (subscriber.SubscribeState == SubscribeState.Subscribe) {
                    return Results.Problem($"Email {email} đã đăng ký nhận thông báo trước đó!");
                }
            }

            return Results.NoContent();
        }

        private static async Task<IResult> Unsubscribe(string email, UnsubscribeEditModel model, ISubscriberRepository subscriberRepository) {
            return await subscriberRepository.UnsubscribeAsync(email, model.Reason)
                ? Results.NoContent()
                : Results.NotFound($"Email {email} chưa đăng ký nhận thông báo!");
        }
    }
}
