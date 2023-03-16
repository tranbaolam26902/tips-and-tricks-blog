using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Core.Entities;
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
        private readonly IValidator<CategoryEditModel> _categoryValidator;

        public CategoriesController(ILogger<PostsController> logger, IBlogRepository blogRepository, IMediaManager mediaManager, IMapper mapper, IValidator<CategoryEditModel> postValidator) {
            _logger = logger;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
            _categoryValidator = postValidator;
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0) {
            var category = id > 0 ? await _blogRepository.GetCategoryByIdAsync(id) : null;
            var model = category == null ? new CategoryEditModel() : _mapper.Map<CategoryEditModel>(category);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditModel model) {
            var validationResult = await _categoryValidator.ValidateAsync(model);

            if (!validationResult.IsValid) {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var category = model.Id > 0 ? await _blogRepository.GetCategoryByIdAsync(model.Id) : null;

            if (category == null) {
                category = _mapper.Map<Category>(model);
                category.Id = 0;
            } else {
                _mapper.Map(model, category);
            }

            await _blogRepository.EditCategoryAsync(category);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCategory(int id) {
            await _blogRepository.DeleteCategoryByIdAsync(id);

            return RedirectToAction("Index");
        }
    }
}
