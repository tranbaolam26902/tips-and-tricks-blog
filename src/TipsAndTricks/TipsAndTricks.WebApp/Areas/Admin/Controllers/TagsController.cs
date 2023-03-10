using Microsoft.AspNetCore.Mvc;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class TagsController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
