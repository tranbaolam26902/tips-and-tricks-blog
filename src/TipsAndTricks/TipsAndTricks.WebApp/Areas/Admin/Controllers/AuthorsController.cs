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
    public class AuthorsController : Controller {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;
        private readonly IValidator<AuthorEditModel> _authorValidator;

        public AuthorsController(ILogger<AuthorsController> logger, IAuthorRepository authorRepository, IMediaManager mediaManager, IMapper mapper, IValidator<AuthorEditModel> authorValidator) {
            _logger = logger;
            _authorRepository = authorRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
            _authorValidator = authorValidator;
        }

        public async Task<IActionResult> Index(
            AuthorFilterModel model,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 10) {
            _logger.LogInformation("Tạo điều kiện truy vấn");

            var authorQuery = _mapper.Map<AuthorQuery>(model);

            _logger.LogInformation("Lấy danh sách tác giả từ CSDL");

            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize
            };

            ViewBag.AuthorsList = await _authorRepository.GetPagedAuthorsByQueryAsync(authorQuery, pagingParams);

            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0) {
            var author = id > 0 ? await _authorRepository.GetAuthorByIdAsync(id) : null;
            var model = author == null ? new AuthorEditModel() : _mapper.Map<AuthorEditModel>(author);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AuthorEditModel model) {
            var validationResult = await _authorValidator.ValidateAsync(model);

            if (!validationResult.IsValid) {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var author = model.Id > 0 ? await _authorRepository.GetAuthorByIdAsync(model.Id) : null;

            if (author == null) {
                author = _mapper.Map<Author>(model);
                author.Id = 0;
                author.JoinedDate = DateTime.Now;
            } else {
                _mapper.Map(model, author);
            }

            if (model.ImageFile?.Length > 0) {
                var newImagePath = await _mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);

                if (!string.IsNullOrWhiteSpace(newImagePath)) {
                    await _mediaManager.DeleteFileAsync(author.ImageUrl);
                    author.ImageUrl = newImagePath;
                }
            }

            await _authorRepository.EditAuthorAsync(author);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteAuthor(int id) {
            await _authorRepository.DeleteAuthorByIdAsync(id);

            return RedirectToAction("Index");
        }
    }
}
