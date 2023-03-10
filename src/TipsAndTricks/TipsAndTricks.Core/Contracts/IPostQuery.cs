namespace TipsAndTricks.Core.Contracts {
    public interface IPostQuery {
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public string AuthorSlug { get; set; }
        public string TagSlug { get; set; }
        public int? PostedYear { get; set; }
        public int? PostedMonth { get; set; }
        public bool Published { get; set; }
        public bool Unpublished { get; set; }
        public string Keyword { get; set; }
    }
}
