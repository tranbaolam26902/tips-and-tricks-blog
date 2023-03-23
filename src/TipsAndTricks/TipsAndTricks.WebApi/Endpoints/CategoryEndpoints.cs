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
using TipsAndTricks.WebApi.Models.Categories;

namespace TipsAndTricks.WebApi.Endpoints {
    public static class CategoryEndpoints {
        public static WebApplication MapCategoryEndpoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategories)
                .WithName("GetCategories")
                .Produces<PaginationResult<CategoryItem>>();
            routeGroupBuilder.MapGet("/{id:int}", GetCategoryById)
                .WithName("GetCategoryById")
                .Produces<CategoryItem>()
                .Produces(404);
            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategorySlug)
                .WithName("GetPostsByCategorySlug")
                .Produces<PaginationResult<PostDTO>>();
            routeGroupBuilder.MapPost("/", AddCategory)
                .WithName("AddNewCategory")
                .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                .Produces(201)
                .Produces(400)
                .Produces(409);

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

        private static async Task<IResult> GetCategoryById(int id, IBlogRepository blogRepository, IMapper mapper) {
            var category = await blogRepository.GetCategoryByIdAsync(id);

            return category == null
                ? Results.NotFound($"Không tìm thấy chủ đề có mã số {id}")
                : Results.Ok(mapper.Map<CategoryItem>(category));
        }

        private static async Task<IResult> GetPostsByCategorySlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var postQuery = new PostQuery() {
                CategorySlug = slug,
                Published = true
            };
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), postQuery, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> AddCategory(CategoryEditModel model, IBlogRepository blogRepository, IMapper mapper) {
            if (await blogRepository.IsCategorySlugExistedAsync(0, model.UrlSlug)) {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var category = mapper.Map<Category>(model);
            await blogRepository.EditCategoryAsync(category);

            return Results.CreatedAtRoute("GetCategoryById", new { category.Id }, mapper.Map<CategoryItem>(category));
        }
    }
}
