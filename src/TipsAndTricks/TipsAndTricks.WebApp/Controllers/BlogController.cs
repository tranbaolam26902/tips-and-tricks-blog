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
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = 1,
                PageSize = 5,
            };
            var postQuery = new PostQuery() {
                Keyword = keyword
            };
            var postsList = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            ViewBag.PostQuery = postQuery;

            return View(postsList);
        }

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}
