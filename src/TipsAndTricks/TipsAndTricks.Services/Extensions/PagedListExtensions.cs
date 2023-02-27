using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TipsAndTricks.Core.Collections;
using TipsAndTricks.Core.Contracts;

namespace TipsAndTricks.Services.Extensions {
    public static class PagedListExtensions {
        public static string GetOrderExpression(this IPagingParams pagingParams, string defaultColumn = "Id") {
            var column = string.IsNullOrWhiteSpace(pagingParams.SortColumn)
                ? defaultColumn
                : pagingParams.SortColumn;
            var order = "ASC".Equals(pagingParams.SortOrder, StringComparison.OrdinalIgnoreCase)
                ? pagingParams.SortOrder
                : "DESC";

            return $"{column} {order}";
        }

        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            var totalItemCount = await source.CountAsync(cancellationToken);
            var items = await source
                .OrderBy(pagingParams.GetOrderExpression())
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Take(pagingParams.PageSize)
                .ToListAsync();

            return new PagedList<T>(items, pagingParams.PageNumber, pagingParams.PageSize, totalItemCount);
        }

        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber = 1, int pageSize = 10, string sortColumn = "Id", string sortOrder = "DESC", CancellationToken cancellationToken = default) {
            var totalItemCount = await source.CountAsync(cancellationToken);
            var items = await source
                .OrderBy($"{sortColumn} {sortOrder}")
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(items, pageNumber, pageSize, totalItemCount);
        }
    }
}