using Microsoft.AspNetCore.Mvc;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class SubscribersController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
