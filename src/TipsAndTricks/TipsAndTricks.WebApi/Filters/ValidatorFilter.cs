using FluentValidation;
using TipsAndTricks.WebApi.Extensions;
using TipsAndTricks.WebApi.Models;

namespace TipsAndTricks.WebApi.Filter {
    public class ValidatorFilter<T> : IEndpointFilter where T : class {
        private readonly IValidator<T> _validator;

        public ValidatorFilter(IValidator<T> validator) {
            _validator = validator;
        }

        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
            var model = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(T)) as T;

            if (model == null) {
                return Results.BadRequest(new ValidationFailureResponse(new[] {
                    "Không thể tạo model object"
                }));
            }

            var validationResult = await _validator.ValidateAsync(model);

            if (!validationResult.IsValid) {
                return Results.BadRequest(validationResult.Errors.ToResponse());
            }

            return await next(context);
        }
    }
}
