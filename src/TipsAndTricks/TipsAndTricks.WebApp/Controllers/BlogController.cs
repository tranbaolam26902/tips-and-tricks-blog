using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Controllers {
    public class BlogController : Controller {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index() {
            var postsList = await _blogRepository.GetPostsAsync();

            return View(postsList);
        }

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}
