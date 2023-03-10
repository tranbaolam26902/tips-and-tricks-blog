using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    }
}
