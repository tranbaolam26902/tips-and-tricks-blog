﻿using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Components {
    public class RandomPostsWidget : ViewComponent {
        private readonly IBlogRepository _blogRepository;

        public RandomPostsWidget(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var posts = await _blogRepository.GetRandomPostsAsync(5);

            return View(posts);
        }
    }
}
