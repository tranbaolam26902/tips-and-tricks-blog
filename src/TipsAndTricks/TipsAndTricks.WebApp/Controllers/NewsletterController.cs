using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Controllers {
    public class NewsletterController : Controller {
        private readonly ISubscriberRepository _subscriberRepository;

        public NewsletterController(ISubscriberRepository subscriberRepository) {
            _subscriberRepository = subscriberRepository;
        }

        #region Subscribe
        public IActionResult Subscribe(string message) {
            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> SubscribeAsync(string email) {
            var isSuccess = await _subscriberRepository.SubscribeAsync(email);

            if (isSuccess)
                return View("SubscribeResult");
            else {
                var subscriber = await _subscriberRepository.GetSubscriberByEmailAsync(email);
                string message = "";

                switch (subscriber.SubscribeState) {
                    case SubscribeState.Banned:
                        message = "Email của bạn đã bị chặn bởi quản trị viên!";
                        break;
                    case SubscribeState.Subscribe:
                        message = "Email này đã đăng ký nhận thông báo!";
                        break;
                };

                return View("Subscribe", message);
            }
        }

        public IActionResult SubscribeResult() {
            return View();
        }
        #endregion

        #region Unsubscribe
        public IActionResult Unsubscribe(string message) {
            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> Unsubscribe(string email, string reason) {
            var isSuccess = await _subscriberRepository.UnsubscribeAsync(email, reason);

            if (isSuccess)
                return View("UnsubscribeResult");
            else {
                var subscriber = await _subscriberRepository.GetSubscriberByEmailAsync(email);
                string message = "";

                if (subscriber == null)
                    message = "Email này chưa được đăng ký!";
                else {
                    switch (subscriber.SubscribeState) {
                        case SubscribeState.Unsubscribe:
                            message = "Email này đã được hủy đăng ký nhận tin trước đó!";
                            break;
                        case SubscribeState.Banned:
                            message = "Email này đã bị chặn bởi quản trị viên!";
                            break;
                    }
                }

                return View("Unsubscribe", message);
            }
        }

        public IActionResult UnsubscribeResult() {
            return View();
        }
        #endregion
    }
}
