using TipsAndTricks.Core.Collections;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.WebApi.Models.Tags;

namespace TipsAndTricks.WebApi.Endpoints {
    public static class TagEndpoints {
        public static WebApplication MapTagEndPoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/tags");

            routeGroupBuilder.MapGet("/", GetTags)
                .WithName("GetTags")
                .Produces<PaginationResult<TagItem>>();

            return app;
        }

        private static async Task<IResult> GetTags([AsParameters] TagFilterModel model, IBlogRepository blogRepository) {
            var tagQuery = new TagQuery() {
                Keyword = model.Name
            };
            var tags = await blogRepository.GetPagedTagsByQueryAsync(tagQuery, model);
            var paginationResult = new PaginationResult<TagItem>(tags);

            return Results.Ok(paginationResult);
        }
    }
}
