using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class DashboardController : Controller {
        private readonly IBlogRepository _blogRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ISubscriberRepository _subscriberRepository;

        public DashboardController(IBlogRepository blogRepository, IAuthorRepository authorRepository, ICommentRepository commentRepository, ISubscriberRepository subscriberRepository) {
            _blogRepository = blogRepository;
            _authorRepository = authorRepository;
            _commentRepository = commentRepository;
            _subscriberRepository = subscriberRepository;
        }

        public async Task<IActionResult> Index() {
            var totalPost = await _blogRepository.CountPostsByQueryAsync(new PostQuery());
            var totalUnpublishedPost = await _blogRepository.CountPostsByQueryAsync(new PostQuery() { Unpublished = true });
            var categories = await _blogRepository.GetCategoriesAsync();
            var authors = await _authorRepository.GetPagedAuthorsByQueryAsync(new AuthorQuery(), new PagingParams());
            var notApprovedComments = await _commentRepository.GetPagedCommentsByQueryAsync(new CommentQuery(), new PagingParams());
            var subscribers = await _subscriberRepository.GetSubscribersAsync();
            var todaySubscribers = await _subscriberRepository.GetPagedSubscribersByQueryAsync(new SubscriberQuery() {
                LastSubscribedDay = DateTime.Now.Day,
                LastSubscribedMonth = DateTime.Now.Month,
                LastSubscribedYear = DateTime.Now.Year
            }, new PagingParams());

            ViewData["TotalPosts"] = totalPost;
            ViewData["TotalUnpublishedPosts"] = totalUnpublishedPost;
            ViewData["TotalCategories"] = categories.Count;
            ViewData["TotalAuthors"] = authors.Count;
            ViewData["TotalNotApprovedComments"] = notApprovedComments.Count;
            ViewData["TotalSubscribers"] = subscribers.Count;
            ViewData["TotalSubscribersToday"] = todaySubscribers.Count;

            return View();
        }
    }
}
