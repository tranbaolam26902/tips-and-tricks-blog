using TipsAndTricks.Core.Contracts;

namespace TipsAndTricks.Services.FilterParams {
    public class AuthorQuery : IAuthorQuery {
        public string Keyword { get; set; }
        public int? JoinedMonth { get; set; }
        public int? JoinedYear { get; set; }
    }
}
