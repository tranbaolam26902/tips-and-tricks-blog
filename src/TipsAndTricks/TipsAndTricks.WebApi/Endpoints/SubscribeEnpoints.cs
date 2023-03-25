namespace TipsAndTricks.WebApi.Endpoints {
    public static class SubscribeEnpoints {
        public static WebApplication MapSubscribeEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/subscribe");

            return app;
        }
    }
}
