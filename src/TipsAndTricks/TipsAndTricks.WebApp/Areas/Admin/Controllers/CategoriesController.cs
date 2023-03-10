using Microsoft.AspNetCore.Mvc;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class CategoriesController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
