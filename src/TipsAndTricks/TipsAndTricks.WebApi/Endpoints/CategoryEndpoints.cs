using TipsAndTricks.Core.Collections;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.WebApi.Models.Categories;

namespace TipsAndTricks.WebApi.Endpoints {
    public static class CategoryEndpoints {
        public static WebApplication MapCategoryEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategories)
               .WithName("GetCategories")
               .Produces<PaginationResult<CategoryItem>>();

            return app;
        }

        private static async Task<IResult> GetCategories([AsParameters] CategoryFilterModel model, IBlogRepository blogRepository) {
            var categoryQuery = new CategoryQuery() {
                Keyword = model.Name
            };
            var categories = await blogRepository.GetPagedCategoriesByQueryAsync(categoryQuery, model);
            var paginationResult = new PaginationResult<CategoryItem>(categories);

            return Results.Ok(paginationResult);
        }
    }
}
