using FluentValidation;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.WebApp.Areas.Admin.Models;

namespace TipsAndTricks.WebApp.Validations {
    public class PostValidator : AbstractValidator<PostEditModel> {
        private readonly IBlogRepository _blogRepository;

        public PostValidator(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(500)
                .WithMessage("Bạn không được để trống tiêu đề bài viết");

            RuleFor(x => x.ShortDescription)
                .NotEmpty()
                .WithMessage("Bạn không được để trống giới thiệu");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Bạn không được để trống nội dung");

            RuleFor(x => x.Meta)
                .NotEmpty()
                .MaximumLength(1000)
                .WithMessage("Bạn không được để trống Metadata");

            RuleFor(x => x.UrlSlug)
                .NotEmpty()
                .MaximumLength(1000)
                .WithMessage("Bạn không được để trống Slug");

            RuleFor(x => x.UrlSlug)
                .MustAsync(async (postModel, slug, cancellationToken) => !await blogRepository
                    .IsPostSlugExistedAsync(slug, cancellationToken))
                    .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn chủ đề cho bài viết");

            RuleFor(x => x.AuthorId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn tác giả của bài viết");

            RuleFor(x => x.SelectedTags)
                .Must(HasAtLeastOneTag)
                .WithMessage("Bạn phải nhập ít nhất một thẻ");

            When(x => x.Id <= 0, () => {
                RuleFor(x => x.ImageFile)
                    .Must(x => x is { Length: > 0 })
                    .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
            })
                .Otherwise(() => {
                    RuleFor(x => x.ImageFile)
                    .MustAsync(SetImageIfNotExist)
                    .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
                });
        }

        private bool HasAtLeastOneTag(PostEditModel postModel, string selectedTags) {
            return postModel.GetSelectedTags().Any();
        }

        private async Task<bool> SetImageIfNotExist(
            PostEditModel postModel, IFormFile imageFile, CancellationToken cancellationToken) {
            var post = await _blogRepository.GetPostByIdAsync(postModel.Id, false, cancellationToken);
            if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
                return true;

            return imageFile is { Length: > 0 };
        }
    }
}
