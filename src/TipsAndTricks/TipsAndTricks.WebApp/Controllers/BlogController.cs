using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Controllers {
    public class BlogController : Controller {
        private readonly IBlogRepository _blogRepository;
        private readonly IAuthorRepository _authorRepository;

        public BlogController(IBlogRepository blogRepository, IAuthorRepository authorRepository) {
            _blogRepository = blogRepository;
            _authorRepository = authorRepository;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name = "keywords")] string keywords,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize
            };
            var postQuery = new PostQuery() {
                Keyword = keywords
            };

            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            ViewBag.PostQuery = postQuery;

            return View(posts);
        }

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");

        public async Task<IActionResult> Category(string slug) {
            var pagingParams = new PagingParams() {
                PageNumber = 1,
                PageSize = 5,
            };
            var postQuery = new PostQuery() {
                CategorySlug = slug
            };

            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            var category = await _blogRepository.GetCategoryBySlugAsync(slug);

            ViewBag.CategoryName = category.Name;
            ViewBag.PostQuery = postQuery;

            return View(posts);
        }

        public async Task<IActionResult> Author(string slug) {
            var pagingParams = new PagingParams() {
                PageNumber = 1,
                PageSize = 5,
            };
            var postQuery = new PostQuery() {
                AuthorSlug = slug
            };
            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            var author = await _authorRepository.GetAuthorBySlugAsync(slug);

            ViewBag.AuthorName = author.FullName;
            ViewBag.PostQuery = postQuery;

            return View(posts);
        }
    }
}
