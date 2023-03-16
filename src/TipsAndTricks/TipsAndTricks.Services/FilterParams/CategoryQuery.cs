using TipsAndTricks.Core.Contracts;

namespace TipsAndTricks.Services.FilterParams {
    public class CategoryQuery : ICategoryQuery {
        public string Keyword { get; set; }
        public bool ShowOnMenu { get; set; }
    }
}
