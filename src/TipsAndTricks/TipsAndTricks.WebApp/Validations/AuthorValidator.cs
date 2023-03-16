using FluentValidation;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.WebApp.Areas.Admin.Models;

namespace TipsAndTricks.WebApp.Validations {
    public class AuthorValidator : AbstractValidator<AuthorEditModel> {
        private readonly IAuthorRepository _authorRepository;

        public AuthorValidator(IAuthorRepository authorRepository) {
            _authorRepository = authorRepository;

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Bạn không được để trống tên tác giả")
                .MaximumLength(100)
                .WithMessage("Tên tác giả không được nhiều hơn 100 ký tự");

            RuleFor(x => x.Email)
                .MaximumLength(150)
                .WithMessage("Email không được nhiều hơn 150 ký tự");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Giới thiệu không được nhiều hơn 500 ký tự");

            RuleFor(x => x.UrlSlug)
                .NotEmpty()
                .WithMessage("Bạn không được để trống slug")
                .MaximumLength(1000)
                .WithMessage("Slug không được nhiều hơn 1000 ký tự");

            RuleFor(x => x.UrlSlug)
                .MustAsync(async (authorModel, slug, cancellationToken) => !await authorRepository
                    .IsAuthorSlugExistedAsync(authorModel.Id, slug, cancellationToken))
                    .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

            When(x => x.Id <= 0, () => {
                RuleFor(x => x.ImageFile)
                    .Must(x => x is { Length: > 0 })
                    .WithMessage("Bạn phải chọn ảnh đại diện cho tác giả");
            })
                .Otherwise(() => {
                    RuleFor(x => x.ImageFile)
                    .MustAsync(SetImageIfNotExist)
                    .WithMessage("Bạn phải chọn ảnh đại diện cho tác giả");
                });
        }

        private async Task<bool> SetImageIfNotExist(
            AuthorEditModel authorModel, IFormFile imageFile, CancellationToken cancellationToken) {
            var post = await _authorRepository.GetAuthorByIdAsync(authorModel.Id, cancellationToken);
            if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
                return true;

            return imageFile is { Length: > 0 };
        }
    }
}
