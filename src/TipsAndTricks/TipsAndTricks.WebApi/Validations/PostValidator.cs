using FluentValidation;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.WebApi.Models.Posts;

namespace TipsAndTricks.WebApi.Validations {
    public class PostValidator : AbstractValidator<PostEditModel> {
        private readonly IBlogRepository _blogRepository;

        public PostValidator(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Bạn không được để trống tiêu đề bài viết")
                .MaximumLength(500)
                .WithMessage("Tiêu đề không được nhiều hơn 500 ký tự");

            RuleFor(x => x.ShortDescription)
                .NotEmpty()
                .WithMessage("Bạn không được để trống giới thiệu");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Bạn không được để trống nội dung");

            RuleFor(x => x.Meta)
                .NotEmpty()
                .WithMessage("Bạn không được để trống metadata")
                .MaximumLength(1000)
                .WithMessage("Metadata không được nhiều hơn 1000 ký tự");

            RuleFor(x => x.UrlSlug)
                .NotEmpty()
                .WithMessage("Bạn không được để trống slug")
                .MaximumLength(1000)
                .WithMessage("Slug không được nhiều hơn 1000 ký tự");

            RuleFor(x => x.UrlSlug)
                .MustAsync(async (postModel, slug, cancellationToken) => !await blogRepository
                    .IsPostSlugExistedAsync(postModel.Id, slug, cancellationToken))
                    .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn chủ đề cho bài viết");

            RuleFor(x => x.AuthorId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn tác giả của bài viết");

            RuleFor(x => x.SelectedTags)
                .NotEmpty()
                .WithMessage("Bạn phải nhập ít nhất một thẻ")
                .Must(HasAtLeastOneTag)
                .WithMessage("Bạn phải nhập ít nhất một thẻ");
        }

        private bool HasAtLeastOneTag(PostEditModel postModel, string selectedTags) {
            return postModel.GetSelectedTags().Any();
        }
    }
}
