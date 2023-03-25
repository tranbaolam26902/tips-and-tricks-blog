using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.WebApi.Models.Dashboard;

namespace TipsAndTricks.WebApi.Endpoints {
    public static class DashboardEndpoints {
        public static WebApplication MapDashboardEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/dashboard");

            routeGroupBuilder.MapGet("/", GetDashboardInformation)
                .WithName("GetDashboardInformation")
                .Produces<DashboardModel>();

            return app;
        }

        private static async Task<IResult> GetDashboardInformation(IBlogRepository blogRepository, IAuthorRepository authorRepository, ICommentRepository commentRepository, ISubscriberRepository subscriberRepository) {
            var totalPost = await blogRepository.CountPostsByQueryAsync(new PostQuery());
            var totalUnpublishedPost = await blogRepository.CountPostsByQueryAsync(new PostQuery() { Unpublished = true });
            var categories = await blogRepository.GetCategoriesAsync();
            var authors = await authorRepository.GetPagedAuthorsByQueryAsync(new AuthorQuery(), new PagingParams());
            var notApprovedComments = await commentRepository.GetPagedCommentsByQueryAsync(new CommentQuery(), new PagingParams());
            var subscribers = await subscriberRepository.GetSubscribersAsync();
            var todaySubscribers = await subscriberRepository.GetPagedSubscribersByQueryAsync(new SubscriberQuery() {
                LastSubscribedDay = DateTime.Now.Day,
                LastSubscribedMonth = DateTime.Now.Month,
                LastSubscribedYear = DateTime.Now.Year
            }, new PagingParams());
            var dashboard = new DashboardModel() {
                TotalPosts = totalPost,
                TotalUnpublishedPosts = totalUnpublishedPost,
                TotalCategories = categories.Count,
                TotalAuthors = authors.Count,
                TotalNotApprovedComments = notApprovedComments.Count,
                TotalSubscribers = subscribers.Count,
                TotalSubscriberToday = todaySubscribers.Count
            };

            return Results.Ok(dashboard);
        }
    }
}
