using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using TipsAndTricks.Services;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Controllers {
    public class BlogController : Controller {
        private readonly IBlogRepository _blogRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public BlogController(IBlogRepository blogRepository, IAuthorRepository authorRepository, ICommentRepository commentRepository, IWebHostEnvironment webHostEnvirontment, IConfiguration configuration) {
            _blogRepository = blogRepository;
            _authorRepository = authorRepository;
            _commentRepository = commentRepository;
            _webHostEnvironment = webHostEnvirontment;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name = "keywords")] string keywords,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize
            };
            var postQuery = new PostQuery() {
                Published = true,
                Keyword = keywords,
            };

            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }

        public IActionResult Contact() => View();

        [HttpPost]
        public IActionResult Contact(string email, string subject, string content) {
            var server = _configuration.GetValue<string>("smtp:server");
            var port = _configuration.GetValue<int>("smtp:port");
            var sender = _configuration.GetValue<string>("smtp:email");
            var password = _configuration.GetValue<string>("smtp:password");
            var adminEmail = _configuration.GetValue<string>("smtp:admin-email");

            using (MailMessage mail = new MailMessage()) {
                mail.From = new MailAddress(sender);
                mail.To.Add(adminEmail);
                mail.Subject = "Ý kiến phản hồi từ " + email;
                mail.Body = "<b>Chủ đề: </b>" + subject + "<br><br>" + "<b>Nội dung:</b><br>" + content.Replace("\n", "<br>");
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(server, port)) {
                    smtp.Credentials = new NetworkCredential(sender, password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            ViewBag.CanGoBack = true;
            ViewData["PageTitle"] = "Liên hệ";
            ViewData["Message"] = "Gửi thành công!";
            ViewData["Description"] = "Cảm ơn bạn đã gửi ý kiến cho chúng tôi";
            ViewData["Image"] = "contacted.jpg";

            return View("Message");
        }

        public IActionResult About() => View();

        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");

        public async Task<IActionResult> Category(
            string slug,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize,
            };
            var postQuery = new PostQuery() {
                Published = true,
                CategorySlug = slug,
            };
            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            var category = await _blogRepository.GetCategoryBySlugAsync(slug);

            ViewData["CategoryName"] = category.Name;
            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }

        public async Task<IActionResult> Author(
            string slug,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize,
            };
            var postQuery = new PostQuery() {
                Published = true,
                AuthorSlug = slug,
            };
            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            var author = await _authorRepository.GetAuthorBySlugAsync(slug);

            ViewData["AuthorName"] = author.FullName;
            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }

        public async Task<IActionResult> Tag(
            string slug,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize,
            };
            var postQuery = new PostQuery() {
                Published = true,
                TagSlug = slug,
            };
            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            var tag = await _blogRepository.GetTagBySlugAsync(slug);

            ViewData["TagName"] = tag.Name;
            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }

        public async Task<IActionResult> Post(int year, int month, int day, string slug) {
            var post = await _blogRepository.GetPostAsync(year, month, slug);
            await _blogRepository.IncreaseViewCountAsync(post.Id);
            post.Comments = await _commentRepository.GetPostCommentsAsync(post.Id);

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Post(string name, string description, int postId) {
            await _commentRepository.SendCommentAsync(name, description, postId);

            ViewBag.CanGoBack = true;
            ViewData["PageTitle"] = "Bình luận";
            ViewData["Message"] = "Gửi thành công!";
            ViewData["Description"] = "Bình luận của bạn đã được gửi đến quản trị viên để phê duyệt.";
            ViewData["Image"] = "commented.jpg";

            return View("Message");
        }

        public async Task<IActionResult> Archive(
            int year,
            int month,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 5) {
            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize,
            };
            var postQuery = new PostQuery() {
                PostedYear = year,
                PostedMonth = month
            };
            var posts = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);

            ViewData["Year"] = year;
            ViewData["Month"] = month;
            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }
    }
}
