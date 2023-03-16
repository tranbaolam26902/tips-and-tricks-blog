using TipsAndTricks.Core.Contracts;

namespace TipsAndTricks.Services.FilterParams {
    public class TagQuery : ITagQuery {
        public string Keyword { get; set; }
    }
}
