namespace TipsAndTricks.WebApi.Models.Authors {
    public class AuthorFilterModel : PagingModel {
        public string Keyword { get; set; }
        public int? JoinedMonth { get; set; }
        public int? JoinedYear { get; set; }
    }
}
