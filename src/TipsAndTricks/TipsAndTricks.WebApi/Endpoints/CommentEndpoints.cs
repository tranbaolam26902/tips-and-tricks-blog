namespace TipsAndTricks.WebApi.Endpoints {
    public static class CommentEndpoints {
        public static WebApplication MapCommentEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/comments");

            return app;
        }
    }
}
