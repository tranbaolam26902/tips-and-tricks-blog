﻿using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Components {
    public class CategoriesWidget : ViewComponent {
        private readonly IBlogRepository _blogRepository;

        public CategoriesWidget(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var categories = await _blogRepository.GetCategoriesAsync(true);

            return View(categories);
        }
    }
}
