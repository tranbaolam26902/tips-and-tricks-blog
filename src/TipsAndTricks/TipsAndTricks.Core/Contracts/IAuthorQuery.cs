namespace TipsAndTricks.Core.Contracts {
    public interface IAuthorQuery {
        public string Keyword { get; set; }
        public int? JoinedMonth { get; set; }
        public int? JoinedYear { get; set; }
    }
}
