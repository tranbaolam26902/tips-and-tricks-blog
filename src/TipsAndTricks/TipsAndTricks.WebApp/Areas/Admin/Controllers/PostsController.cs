using Microsoft.AspNetCore.Mvc;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class PostsController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
