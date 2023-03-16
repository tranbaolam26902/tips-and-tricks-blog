using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.Services.Media;
using TipsAndTricks.WebApp.Areas.Admin.Models;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class CategoriesController : Controller {
        private readonly ILogger<PostsController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;
        private readonly IValidator<PostEditModel> _postValidator;

        public CategoriesController(ILogger<PostsController> logger, IBlogRepository blogRepository, IMediaManager mediaManager, IMapper mapper, IValidator<PostEditModel> postValidator) {
            _logger = logger;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
            _postValidator = postValidator;
        }

        public async Task<IActionResult> Index(
            CategoryFilterModel model,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 10) {
            _logger.LogInformation("Tạo điều kiện truy vấn");

            var categoryQuery = _mapper.Map<CategoryQuery>(model);

            _logger.LogInformation("Lấy danh sách chủ đề từ CSDL");

            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize
            };

            ViewBag.CategoriesList = await _blogRepository.GetPagedCategoriesByQueryAsync(categoryQuery, pagingParams);

            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            return View(model);
        }
    }
}
