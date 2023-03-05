namespace TipsAndTricks.Core.Contracts {
    public interface IPostQuery {
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public int PostedYear { get; set; }
        public int PostedMonth { get; set; }
    }
}
