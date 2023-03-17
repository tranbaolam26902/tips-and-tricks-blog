using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.Services.Media;
using TipsAndTricks.WebApp.Areas.Admin.Models;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class CommentsController : Controller {

        private readonly ILogger<CommentsController> _logger;
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;

        public CommentsController(ILogger<CommentsController> logger, ICommentRepository commentRepository, IBlogRepository blogRepository, IMediaManager mediaManager, IMapper mapper) {
            _logger = logger;
            _commentRepository = commentRepository;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
        }

        private async Task PopulateCommentFilterModelAsync(CommentFilterModel model) {
            var posts = await _blogRepository.GetPostsAsync();

            model.PostList = posts.Select(p => new SelectListItem() {
                Text = p.Title,
                Value = p.Id.ToString()
            });
        }

        public async Task<IActionResult> Index(
            CommentFilterModel model,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 10) {
            _logger.LogInformation("Tạo điều kiện truy vấn");

            var commentQuery = _mapper.Map<CommentQuery>(model);

            _logger.LogInformation("Lấy danh sách bình luận từ CSDL");

            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize
            };

            ViewBag.CommentsList = await _commentRepository.GetPagedCommentsByQueryAsync(commentQuery, pagingParams);

            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            await PopulateCommentFilterModelAsync(model);

            return View(model);
        }

        public async Task<IActionResult> ChangeCommentApprovedState(int id) {
            await _commentRepository.ChangeCommentApprovedState(id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteComment(int id) {
            await _commentRepository.DeleteCommentByIdAsync(id);

            return RedirectToAction("Index");
        }
    }
}
