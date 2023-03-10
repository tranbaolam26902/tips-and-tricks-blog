using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TipsAndTricks.Services;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.WebApp.Areas.Admin.Models;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class PostsController : Controller {
        private readonly IBlogRepository _blogRepository;

        public PostsController(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;
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
            var postQuery = new PostQuery() {
                Keyword = model.Keyword,
                CategoryId = model.CategoryId,
                AuthorId = model.AuthorId,
                PostedYear = model.PostedYear,
                PostedMonth = model.PostedMonth
            };
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
