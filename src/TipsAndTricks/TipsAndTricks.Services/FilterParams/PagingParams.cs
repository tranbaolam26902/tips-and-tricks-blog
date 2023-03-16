using TipsAndTricks.Core.Contracts;

namespace TipsAndTricks.Services.FilterParams {
    public class PagingParams : IPagingParams {
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public string SortColumn { get; set; } = "Id";
        public string SortOrder { get; set; } = "DESC";
    }
}
