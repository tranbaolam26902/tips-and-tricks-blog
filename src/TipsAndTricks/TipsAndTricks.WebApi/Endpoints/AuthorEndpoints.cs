using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TipsAndTricks.Core.Collections;
using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.Services.Media;
using TipsAndTricks.WebApi.Filter;
using TipsAndTricks.WebApi.Models;
using TipsAndTricks.WebApi.Models.Authors;
using TipsAndTricks.WebApi.Models.Posts;

namespace TipsAndTricks.WebApi.Endpoints {
    public static class AuthorEndpoints {
        public static WebApplication MapAuthorEndPoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/authors");

            routeGroupBuilder.MapGet("/", GetAuthors)
                .WithName("GetAuthors")
                .Produces<ApiResponse<PaginationResult<AuthorItem>>>();

            routeGroupBuilder.MapGet("/best/{limit:int}", GetAuthorsHasMostArticles)
                .WithDescription("GetBestAuthors")
                .Produces<ApiResponse<IPagedList<AuthorItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
                .WithName("GetAuthorById")
                .Produces<ApiResponse<AuthorItem>>()
                .Produces(404);

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByAuthorSlug)
                .WithName("GetPostsByAuthorSlug")
                .Produces<ApiResponse<PaginationResult<PostDTO>>>();

            routeGroupBuilder.MapPost("/", AddAuthor)
                .WithName("AddNewAuthor")
                .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<AuthorItem>>();

            routeGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
                .WithName("SetAuthorPicture")
                .Accepts<IFormFile>("multipart/form-data")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
                .WithName("UpdateAnAuthor")
                .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
                .WithName("DeleteAnAuthor")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetAuthors([AsParameters] AuthorFilterModel model, IAuthorRepository authorRepository) {
            var authors = await authorRepository.GetPagedAuthorsAsync(model, model.Name);
            var paginationResult = new PaginationResult<AuthorItem>(authors);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetAuthorsHasMostArticles(int limit, IAuthorRepository authorRepository) {
            var authors = await authorRepository.GetAuthorsHasMostArticles(limit);

            return Results.Ok(ApiResponse.Success(authors));
        }

        private static async Task<IResult> GetAuthorDetails(int id, IAuthorRepository authorRepository, IMapper mapper) {
            var author = await authorRepository.GetCachedAuthorByIdAsync(id);

            return author == null
                ? Results.NotFound(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
        }

        private static async Task<IResult> GetPostsByAuthorId(int id, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var postQuery = new PostQuery() {
                AuthorId = id,
                Published = true
            };
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), postQuery, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPostsByAuthorSlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var postQuery = new PostQuery() {
                AuthorSlug = slug,
                Published = true
            };
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), postQuery, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> AddAuthor(AuthorEditModel model, IAuthorRepository authorRepository, IMapper mapper) {
            if (await authorRepository.IsAuthorSlugExistedAsync(0, model.UrlSlug)) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var author = mapper.Map<Author>(model);
            await authorRepository.EditAuthorAsync(author);

            return Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author), HttpStatusCode.Created));
        }

        private static async Task<IResult> SetAuthorPicture(int id, IFormFile imageFile, IAuthorRepository authorRepository, IMediaManager mediaManager) {
            var imageUrl = await mediaManager.SaveFileAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl)) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await authorRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        private static async Task<IResult> UpdateAuthor(int id, AuthorEditModel model, IAuthorRepository authorRepository, IMapper mapper) {
            if (await authorRepository.IsAuthorSlugExistedAsync(id, model.UrlSlug)) {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var author = mapper.Map<Author>(model);
            author.Id = id;

            return await authorRepository.EditAuthorAsync(author) != null
                ? Results.Ok(ApiResponse.Success("Cập nhật thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy tác giả có mã số {id}"));
        }

        private static async Task<IResult> DeleteAuthor(int id, IAuthorRepository authorRepository) {
            return await authorRepository.DeleteAuthorByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Xóa thành công", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không thể tìm thấy tác giả với id = {id}"));
        }
    }
}