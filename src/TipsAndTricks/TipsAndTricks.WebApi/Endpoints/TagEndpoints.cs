using MapsterMapper;
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
            routeGroupBuilder.MapGet("/{id:int}", GetTagById)
                .WithName("GetTagById")
                .Produces<TagItem>()
                .Produces(404);

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

        private static async Task<IResult> GetTagById(int id, IBlogRepository blogRepository, IMapper mapper) {
            var tag = await blogRepository.GetTagByIdAsync(id);

            return tag == null
                ? Results.NotFound($"Không tìm thấy thẻ có mã số {id}")
                : Results.Ok(mapper.Map<TagItem>(tag));
        }
    }
}
