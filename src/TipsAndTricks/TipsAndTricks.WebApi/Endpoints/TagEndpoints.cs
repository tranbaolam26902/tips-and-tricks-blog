using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Core.Collections;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.WebApi.Filter;
using TipsAndTricks.WebApi.Models;
using TipsAndTricks.WebApi.Models.Posts;
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
            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetTagPostsBySlug)
                .WithName("GetTagPostsBySlug")
                .Produces<PaginationResult<PostDTO>>();
            routeGroupBuilder.MapPost("/", AddTag)
                .WithName("AddNewTag")
                .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
                .Produces(201)
                .Produces(400)
                .Produces(409);
            routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
                .WithName("UpdateATag")
                .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
                .Produces(201)
                .Produces(400)
                .Produces(409);
            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
                .WithName("DeleteTag")
                .Produces(204)
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

        private static async Task<IResult> GetTagPostsBySlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var postQuery = new PostQuery() {
                TagSlug = slug,
                Published = true
            };
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), postQuery, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> AddTag(TagEditModel model, IBlogRepository blogRepository, IMapper mapper) {
            if (await blogRepository.IsTagSlugExistedAsync(0, model.UrlSlug)) {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var tag = mapper.Map<Tag>(model);
            await blogRepository.EditTagAsync(tag);

            return Results.CreatedAtRoute("GetTagById", new { tag.Id }, mapper.Map<TagItem>(tag));
        }

        private static async Task<IResult> UpdateTag(int id, TagEditModel model, IBlogRepository blogRepository, IMapper mapper) {
            if (await blogRepository.IsTagSlugExistedAsync(id, model.UrlSlug)) {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var tag = mapper.Map<Tag>(model);
            tag.Id = id;

            return await blogRepository.EditTagAsync(tag) != null
                ? Results.NoContent()
                : Results.NotFound();
        }

        private static async Task<IResult> DeleteTag(int id, IBlogRepository blogRepository) {
            return await blogRepository.DeleteTagByIdAsync(id)
                ? Results.NoContent()
                : Results.NotFound($"Không tìm thấy thẻ có mã số {id}");
        }
    }
}
