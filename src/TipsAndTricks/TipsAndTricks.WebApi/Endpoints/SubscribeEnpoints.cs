using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.WebApi.Models;
using TipsAndTricks.WebApi.Models.Subscribers;

namespace TipsAndTricks.WebApi.Endpoints {
    public static class SubscribeEnpoints {
        public static WebApplication MapSubscribeEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/subscribe");

            routeGroupBuilder.MapGet("/", GetSubscribers)
                .WithName("GetSubscribers")
                .Produces<IPagedList<Subscriber>>();
            routeGroupBuilder.MapPut("/{id:int}", BanSubscriber)
                .WithName("BanSubscriber")
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
    }
}
