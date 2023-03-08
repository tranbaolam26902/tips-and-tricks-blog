﻿using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Components {
    public class FeaturedPostsWidget : ViewComponent {
        private readonly IBlogRepository _blogRepository;

        public FeaturedPostsWidget(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var posts = await _blogRepository.GetPopularArticlesAsync(3);

            return View(posts);
        }
    }
}
