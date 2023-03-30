using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
                .Produces<ApiResponse<PaginationResult<TagItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetTagById)
                .WithName("GetTagById")
                .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetTagPostsBySlug)
                .WithName("GetTagPostsBySlug")
                .Produces<ApiResponse<PaginationResult<PostDTO>>>();

            routeGroupBuilder.MapPost("/", AddTag)
                .WithName("AddNewTag")
                .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
                .WithName("UpdateATag")
                .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
                .WithName("DeleteTag")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetTags([AsParameters] TagFilterModel model, IBlogRepository blogRepository) {
            var tagQuery = new TagQuery() {
                Keyword = model.Name
            };
            var tags = await blogRepository.GetPagedTagsByQueryAsync(tagQuery, model);
            var paginationResult = new PaginationResult<TagItem>(tags);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetTagById(int id, IBlogRepository blogRepository, IMapper mapper) {
            var tag = await blogRepository.GetTagByIdAsync(id);

            return tag == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy thẻ có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag)));
        }

        private static async Task<IResult> GetTagPostsBySlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var postQuery = new PostQuery() {
                TagSlug = slug,
                Published = true
            };
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), postQuery, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> AddTag(TagEditModel model, IBlogRepository blogRepository, IMapper mapper) {
            if (await blogRepository.IsTagSlugExistedAsync(0, model.UrlSlug)) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var tag = mapper.Map<Tag>(model);
            await blogRepository.EditTagAsync(tag);

            return Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateTag(int id, TagEditModel model, IBlogRepository blogRepository, IMapper mapper) {
            if (await blogRepository.IsTagSlugExistedAsync(id, model.UrlSlug)) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var tag = mapper.Map<Tag>(model);
            tag.Id = id;

            return await blogRepository.EditTagAsync(tag) != null
                ? Results.Ok(ApiResponse.Success("Cập nhật thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy thẻ có mã số {id}"));
        }

        private static async Task<IResult> DeleteTag(int id, IBlogRepository blogRepository) {
            return await blogRepository.DeleteTagByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Xóa thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy thẻ có mã số {id}"));
        }
    }
}
