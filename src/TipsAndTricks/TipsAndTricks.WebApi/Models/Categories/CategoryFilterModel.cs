namespace TipsAndTricks.WebApi.Models.Categories {
    public class CategoryFilterModel : PagingModel {
        public string Keyword { get; set; }
        public bool ShowOnMenu { get; set; }
    }
}
