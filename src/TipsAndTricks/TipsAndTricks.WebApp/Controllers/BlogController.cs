using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Controllers {
    public class BlogController : Controller {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;
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
            var postsList = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            ViewBag.PostQuery = postQuery;

            return View(postsList);
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
            var postsList = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            var category = await _blogRepository.GetCategoryBySlugAsync(slug);
            ViewBag.CategoryName = category.Name;
            ViewBag.PostQuery = postQuery;

            return View(postsList);
        }
    }
}
