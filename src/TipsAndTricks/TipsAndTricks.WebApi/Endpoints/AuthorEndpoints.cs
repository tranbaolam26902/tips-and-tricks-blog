using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
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

namespace TipsAndTricks.WebApi.Endpoints {
    public static class AuthorEndpoints {
        public static WebApplication MapAuthorEndPoints(this WebApplication app) {
            var routeGroupBuilder = app.MapGroup("/api/authors");

            routeGroupBuilder.MapGet("/", GetAuthors)
                .WithName("GetAuthors")
                .Produces<PaginationResult<AuthorItem>>();
            routeGroupBuilder.MapGet("/best/{limit:int}", GetAuthorsHasMostArticles)
                .WithDescription("GetBestAuthors")
                .Produces<IPagedList<AuthorItem>>();
            routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
                .WithName("GetAuthorById")
                .Produces<AuthorItem>()
                .Produces(404);
            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByAuthorSlug)
                .WithName("GetPostsByAuthorSlug")
                .Produces<PaginationResult<Post>>();
            routeGroupBuilder.MapPost("/", AddAuthor)
                .WithName("AddNewAuthor")
                .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
                .Produces(201)
                .Produces(400)
                .Produces(409);
            routeGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
                .WithName("SetAuthorPicture")
                .Accepts<IFormFile>("multipart/form-data")
                .Produces<string>()
                .Produces(404);
            routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
                .WithName("UpdateAnAuthor")
                .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
                .Produces(204)
                .Produces(400)
                .Produces(409);
            routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
                .WithName("DeleteAnAuthor")
                .Produces(204)
                .Produces(404);

            return app;
        }

        private static async Task<IResult> GetAuthors([AsParameters] AuthorFilterModel model, IAuthorRepository authorRepository) {
            var authors = await authorRepository.GetPagedAuthorsAsync(model, model.Name);
            var paginationResult = new PaginationResult<AuthorItem>(authors);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetAuthorsHasMostArticles(int limit, IAuthorRepository authorRepository) {
            var authors = await authorRepository.GetAuthorsHasMostArticles(limit);

            return Results.Ok(authors);
        }

        private static async Task<IResult> GetAuthorDetails(int id, IAuthorRepository authorRepository, IMapper mapper) {
            var author = await authorRepository.GetCachedAuthorByIdAsync(id);

            return author == null
                ? Results.NotFound($"Không tìm thấy tác giả có mã số {id}")
                : Results.Ok(mapper.Map<AuthorItem>(author));
        }

        private static async Task<IResult> GetPostsByAuthorId(int id, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var postQuery = new PostQuery() {
                AuthorId = id,
                Published = true
            };
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), postQuery, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetPostsByAuthorSlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository) {
            var postQuery = new PostQuery() {
                AuthorSlug = slug,
                Published = true
            };
            var posts = await blogRepository.GetPagedPostsByQueryAsync(posts => posts.ProjectToType<PostDTO>(), postQuery, pagingModel);
            var paginationResult = new PaginationResult<PostDTO>(posts);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> AddAuthor(AuthorEditModel model, IAuthorRepository authorRepository, IMapper mapper) {
            if (await authorRepository.IsAuthorSlugExistedAsync(0, model.UrlSlug)) {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var author = mapper.Map<Author>(model);
            await authorRepository.EditAuthorAsync(author);

            return Results.CreatedAtRoute("GetAuthorById", new { author.Id }, mapper.Map<AuthorItem>(author));
        }

        private static async Task<IResult> SetAuthorPicture(int id, IFormFile imageFile, IAuthorRepository authorRepository, IMediaManager mediaManager) {
            var imageUrl = await mediaManager.SaveFileAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl)) {
                return Results.BadRequest("Không lưu được tập tin");
            }

            await authorRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(imageUrl);
        }

        private static async Task<IResult> UpdateAuthor(int id, AuthorEditModel model, IAuthorRepository authorRepository, IMapper mapper) {
            if (await authorRepository.IsAuthorSlugExistedAsync(id, model.UrlSlug)) {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var author = mapper.Map<Author>(model);
            author.Id = id;

            return await authorRepository.EditAuthorAsync(author) != null
                ? Results.NoContent()
                : Results.NotFound();
        }

        private static async Task<IResult> DeleteAuthor(int id, IAuthorRepository authorRepository) {
            return await authorRepository.DeleteAuthorByIdAsync(id)
                ? Results.NoContent()
                : Results.NotFound($"Không thể tìm thấy tác giả với id = {id}");
        }
    }
}