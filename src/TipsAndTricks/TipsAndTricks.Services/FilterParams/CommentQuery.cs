using TipsAndTricks.Core.Contracts;

namespace TipsAndTricks.Services.FilterParams {
    public class CommentQuery : ICommentQuery {
        public string Keyword { get; set; }
        public int? PostedMonth { get; set; }
        public int? PostedYear { get; set; }
        public bool IsNotApproved { get; set; }
        public int? PostId { get; set; }
    }
}
