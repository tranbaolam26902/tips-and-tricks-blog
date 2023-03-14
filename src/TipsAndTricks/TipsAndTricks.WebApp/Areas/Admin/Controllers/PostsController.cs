using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.WebApp.Areas.Admin.Models;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class PostsController : Controller {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public PostsController(IBlogRepository blogRepository, IMapper mapper) {
            _blogRepository = blogRepository;
            _mapper = mapper;

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

        public async Task<IActionResult> Index(PostFilterModel model) {
            var postQuery = _mapper.Map<PostQuery>(model);
            var pagingParams = new PagingParams() {
                PageNumber = 1,
                PageSize = 10
            };

            ViewBag.PostsList = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);

            await PopulatePostFilterModelAsync(model);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0) {
            var post = id > 0 ? await _blogRepository.GetPostByIdAsync(id) : null;
            var model = post == null ? new PostEditModel() : _mapper.Map<PostEditModel>(post);

            await PopulatePostEditModelAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostEditModel model) {
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

            await _blogRepository.EditPostAsync(post, model.GetSelectedTags());

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPostSlug(string slug) {
            var slugExisted = await _blogRepository.IsPostSlugExistedAsync(slug);

            return slugExisted ? Json($"Slug '{slug}' đã được sử dụng") : Json(true);
        }
    }
}
