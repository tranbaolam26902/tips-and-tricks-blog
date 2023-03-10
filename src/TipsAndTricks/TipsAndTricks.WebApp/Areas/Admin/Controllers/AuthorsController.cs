using Microsoft.AspNetCore.Mvc;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class AuthorsController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
