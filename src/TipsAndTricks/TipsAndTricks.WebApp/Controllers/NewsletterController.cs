using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Controllers {
    public class NewsletterController : Controller {
        private readonly ISubscriberRepository _subscriberRepository;

        public NewsletterController(ISubscriberRepository subscriberRepository) {
            _subscriberRepository = subscriberRepository;
        }

        #region Subscribe
        public IActionResult Subscribe() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string email) {
            await _subscriberRepository.SubscribeAsync(email);

            return RedirectToAction("SubscribeResult");
        }

        public IActionResult SubscribeResult() {
            return View();
        }
        #endregion

        #region Unsubscribe
        public IActionResult Unsubscribe() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Unsubscribe(string email, string reason) {
            await _subscriberRepository.UnsubscribeAsync(email, reason);

            return RedirectToAction("UnsubscribeResult");
        }

        public IActionResult UnsubscribeResult() {
            return View();
        }
        #endregion
    }
}
