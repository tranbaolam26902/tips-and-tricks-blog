using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.Services.Media;
using TipsAndTricks.WebApp.Areas.Admin.Models;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class PostsController : Controller {
        private readonly ILogger<PostsController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;
        private readonly IValidator<PostEditModel> _postValidator;

        public PostsController(ILogger<PostsController> logger, IBlogRepository blogRepository, IMediaManager mediaManager, IMapper mapper, IValidator<PostEditModel> postValidator) {
            _logger = logger;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
            _postValidator = postValidator;
        }

        private async Task PopulatePostFilterModelAsync(PostFilterModel model) {
            var authors = await _blogRepository.GetAuthorsAsync();
            var categories = await _blogRepository.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem() {
                Text = a.FullName,
                Value = a.Id.ToString()
            });

            model.CategoryList = categories.Select(c => new SelectListItem() {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        private async Task PopulatePostEditModelAsync(PostEditModel model) {
            var authors = await _blogRepository.GetAuthorsAsync();
            var categories = await _blogRepository.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem() {
                Text = a.FullName,
                Value = a.Id.ToString()
            });

            model.CategoryList = categories.Select(c => new SelectListItem() {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        public async Task<IActionResult> Index(
            PostFilterModel model,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 10) {
            _logger.LogInformation("Tạo điều kiện truy vấn");

            var postQuery = _mapper.Map<PostQuery>(model);

            _logger.LogInformation("Lấy danh sách bài viết từ CSDL");

            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize
            };

            ViewBag.PostsList = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);

            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            await PopulatePostFilterModelAsync(model);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0) {
            var post = id > 0 ? await _blogRepository.GetPostByIdAsync(id, true) : null;
            var model = post == null ? new PostEditModel() : _mapper.Map<PostEditModel>(post);

            await PopulatePostEditModelAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostEditModel model) {
            var validationResult = await _postValidator.ValidateAsync(model);

            if (!validationResult.IsValid) {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var post = model.Id > 0 ? await _blogRepository.GetPostByIdAsync(model.Id) : null;

            if (post == null) {
                post = _mapper.Map<Post>(model);
                post.Id = 0;
                post.PostedDate = DateTime.Now;
            } else {
                _mapper.Map(model, post);
                post.Category = null;
                post.ModifiedDate = DateTime.Now;
            }

            if (model.ImageFile?.Length > 0) {
                var newImagePath = await _mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);

                if (!string.IsNullOrWhiteSpace(newImagePath)) {
                    await _mediaManager.DeleteFileAsync(post.ImageUrl);
                    post.ImageUrl = newImagePath;
                }
            }

            await _blogRepository.EditPostAsync(post, model.GetSelectedTags());

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPostSlug(int id, string slug) {
            var slugExisted = await _blogRepository.IsPostSlugExistedAsync(id, slug);

            return slugExisted ? Json($"Slug '{slug}' đã được sử dụng") : Json(true);
        }

        public async Task<IActionResult> ChangePostPublishedStatus(int id) {
            await _blogRepository.ChangePostPublishedStatusAsync(id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeletePost(int id) {
            await _blogRepository.DeletePostById(id);

            return RedirectToAction("Index");
        }
    }
}
