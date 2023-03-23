namespace TipsAndTricks.WebApi.Endpoints {
    public static class CategoryEndpoints {
        public static WebApplication MapCategoryEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            return app;
        }
    }
}
