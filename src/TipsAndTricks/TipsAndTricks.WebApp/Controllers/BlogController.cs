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
                Published = true,
                Keyword = keywords,
            };

            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }

        public IActionResult Contact() => View();

        public IActionResult About() => View();

        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");

        public async Task<IActionResult> Category(
            string slug,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize,
            };
            var postQuery = new PostQuery() {
                Published = true,
                CategorySlug = slug,
            };

            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            var category = await _blogRepository.GetCategoryBySlugAsync(slug);

            ViewData["CategoryName"] = category.Name;
            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }

        public async Task<IActionResult> Author(
            string slug,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize,
            };
            var postQuery = new PostQuery() {
                Published = true,
                AuthorSlug = slug,
            };
            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            var author = await _authorRepository.GetAuthorBySlugAsync(slug);

            ViewData["AuthorName"] = author.FullName;
            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }

        public async Task<IActionResult> Tag(
            string slug,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize,
            };
            var postQuery = new PostQuery() {
                Published = true,
                TagSlug = slug,
            };
            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            var tag = await _blogRepository.GetTagBySlugAsync(slug);

            ViewData["TagName"] = tag.Name;
            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }

        public async Task<IActionResult> Post(int year, int month, int day, string slug) {
            var post = await _blogRepository.GetPostAsync(year, month, slug);
            await _blogRepository.IncreaseViewCountAsync(post.Id);

            return View(post);
        }

        public async Task<IActionResult> Archive(
            int year,
            int month,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize,
            };
            var postQuery = new PostQuery() {
                PostedYear = year,
                PostedMonth = month
            };
            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);

            ViewData["Year"] = year;
            ViewData["Month"] = month;
            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }
    }
}
