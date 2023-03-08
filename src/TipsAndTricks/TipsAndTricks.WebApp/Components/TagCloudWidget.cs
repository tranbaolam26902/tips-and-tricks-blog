﻿using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Components {
    public class TagCloudWidget : ViewComponent {
        private readonly IBlogRepository _blogRepository;

        public TagCloudWidget(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var tags = await _blogRepository.GetTagsAsync();

            return View(tags);
        }
    }
}
