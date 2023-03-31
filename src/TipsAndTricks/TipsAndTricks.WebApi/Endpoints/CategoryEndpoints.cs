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
using TipsAndTricks.WebApi.Models.Categories;
using TipsAndTricks.WebApi.Models.Posts;

namespace TipsAndTricks.WebApi.Endpoints {
    public static class CategoryEndpoints {
        public static WebApplication MapCategoryEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategories)
                .WithName("GetCategories")
                .Produces<ApiResponse<PaginationResult<CategoryItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCategoryById)
                .WithName("GetCategoryById")
                .Produces<ApiResponse<CategoryItem>>();

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}", GetCategoryBySlug)
                .WithName("GetCategoryBySlug")
                .Produces<ApiResponse<CategoryItem>>();

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategorySlug)
                .WithName("GetPostsByCategorySlug")
                .Produces<ApiResponse<PaginationResult<PostDTO>>>();

            routeGroupBuilder.MapPost("/", AddCategory)
                .WithName("AddNewCategory")
                .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<CategoryItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
                .WithName("UpdateAnCategory")
                .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
                .WithName("DeleteCategory")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetCategories([AsParameters] CategoryFilterModel model, IBlogRepository blogRepository) {
            var categoryQuery = new CategoryQuery() {
                Keyword = model.Name
            };
            var categories = await blogRepository.GetPagedCategoriesByQueryAsync(categoryQuery, model);
            var paginationResult = new PaginationResult<CategoryItem>(categories);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetCategoryById(int id, IBlogRepository blogRepository, IMapper mapper) {
            var category = await blogRepository.GetCategoryByIdAsync(id);

            return category == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy chủ đề có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
        }

        private static async Task<IResult> GetCategoryBySlug(string slug, IBlogRepository blogRepository, IMapper mapper) {
            var category = await blogRepository.GetCategoryBySlugAsync(slug);

            return category == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy chủ đề có slug '{slug}'"))
                : Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
        }

        private static async Task<IResult> GetPostsByCategorySlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var postQuery = new PostQuery() {
                CategorySlug = slug,
                Published = true
            };
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), postQuery, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> AddCategory(CategoryEditModel model, IBlogRepository blogRepository, IMapper mapper) {
            if (await blogRepository.IsCategorySlugExistedAsync(0, model.UrlSlug)) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var category = mapper.Map<Category>(model);
            await blogRepository.EditCategoryAsync(category);

            return Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateCategory(int id, CategoryEditModel model, IBlogRepository blogRepository, IMapper mapper) {
            if (await blogRepository.IsCategorySlugExistedAsync(id, model.UrlSlug)) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var category = mapper.Map<Category>(model);
            category.Id = id;

            return await blogRepository.EditCategoryAsync(category) != null
                ? Results.Ok(ApiResponse.Success("Cập nhật thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy chủ đề có mã số {id}"));
        }

        private static async Task<IResult> DeleteCategory(int id, IBlogRepository blogRepository) {
            return await blogRepository.DeleteCategoryByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Xóa thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy chủ đề có mã số {id}"));
        }
    }
}
