using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Controllers {
    public class NewsletterController : Controller {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public NewsletterController(ISubscriberRepository subscriberRepository, IWebHostEnvironment webHostEnvironment, IConfiguration configuration) {
            _subscriberRepository = subscriberRepository;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        #region Subscribe
        public IActionResult Subscribe(string message) {
            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> SubscribeAsync(string email) {
            var isSuccess = await _subscriberRepository.SubscribeAsync(email);

            if (isSuccess) {
                var subscriber = await _subscriberRepository.GetSubscriberByEmailAsync(email);
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string filePath = Path.Combine(wwwrootPath, "templates/emails/Subscribed.html");
                string fileContents = System.IO.File.ReadAllText(filePath);

                fileContents = fileContents.Replace("{link}", "https://localhost:7164/Newsletter/Unsubscribe");
                fileContents = fileContents.Replace("{email}", subscriber.Email);

                var server = _configuration.GetValue<string>("smtp:server");
                var port = _configuration.GetValue<int>("smtp:port");
                var sender = _configuration.GetValue<string>("smtp:email");
                var password = _configuration.GetValue<string>("smtp:password");

                using (MailMessage mail = new MailMessage()) {
                    mail.From = new MailAddress(sender);
                    mail.To.Add(subscriber.Email);
                    mail.Subject = $"Xác nhận đăng ký nhận thông báo từ Cats & Tricks Blog";
                    mail.Body = fileContents;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(server, port)) {
                        smtp.Credentials = new NetworkCredential(sender, password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                ViewBag.CanGoBack = false;
                ViewData["PageTitle"] = "Đăng ký nhận thông báo";
                ViewData["Message"] = "Cảm ơn bạn đã đăng ký!";
                ViewData["Description"] = "Chúng tôi sẽ gửi thông báo cho bạn qua email khi có nội dung mới.";
                ViewData["Image"] = "subscribed.jpg";

                return View("Message");
            } else {
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
        #endregion

        #region Unsubscribe
        public IActionResult Unsubscribe(string message) {
            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> Unsubscribe(string email, string reason) {
            var isSuccess = await _subscriberRepository.UnsubscribeAsync(email, reason);

            if (isSuccess) {

                ViewBag.CanGoBack = false;
                ViewData["PageTitle"] = "Hủy đăng ký nhận thông báo";
                ViewData["Message"] = "Huỷ đăng ký thành công!";
                ViewData["Description"] = "Cảm ơn bạn đã theo dõi trong thời gian qua :(";
                ViewData["Image"] = "unsubscribed.jpg";

                return View("Message");
            } else {
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
        #endregion
    }
}
