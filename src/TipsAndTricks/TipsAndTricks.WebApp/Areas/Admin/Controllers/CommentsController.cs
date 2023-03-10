using Microsoft.AspNetCore.Mvc;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class CommentsController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
