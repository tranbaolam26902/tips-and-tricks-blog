using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Services.Blogs;

namespace TipsAndTricks.WebApp.Components {
    public class BestAuthorsWidget : ViewComponent {
        private readonly IAuthorRepository _authorRepository;

        public BestAuthorsWidget(IAuthorRepository authorRepository) {
            _authorRepository = authorRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var authors = await _authorRepository.GetAuthorsHasMostArticles(4);

            return View(authors);
        }
    }
}
