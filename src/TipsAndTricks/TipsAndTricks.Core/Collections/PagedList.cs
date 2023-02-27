using System.Collections;
using TipsAndTricks.Core.Contracts;

namespace TipsAndTricks.Core.Collections {
    public class PagedList<T> : IPagedList<T> {
        private readonly List<T> _subset = new();

        public PagedList(IList<T> items, int pageNumber, int pageSize, int totalItemCount) {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
            _subset.AddRange(items);
        }

        public T this[int index] => _subset[index];

        public virtual int Count => _subset.Count;

        public int PageCount {
            get {
                if (PageSize == 0)
                    return 0;
                var total = TotalItemCount / PageSize;
                if (TotalItemCount % PageSize > 0)
                    total++;
                return total;
            }
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItemCount { get; }

        public int PageNumber { get => PageIndex + 1; set => PageIndex = value - 1; }

        public bool HasPreviousPage { get => PageIndex > 0; }

        public bool HasNextPage { get => PageIndex < (PageCount - 1); }

        public bool IsFirstPage { get => PageIndex <= 0; }

        public bool IsLastPage { get => PageIndex >= (PageCount - 1); }

        public int FirstItemIndex { get => (PageIndex * PageSize) + 1; }

        public int LastItemIndex { get => Math.Min(TotalItemCount, ((PageIndex * PageSize) + PageSize)); }

        public IEnumerator<T> GetEnumerator() {
            return _subset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
