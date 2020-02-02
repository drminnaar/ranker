using System.Linq;

namespace System.Collections.Generic
{
    [Serializable]
    public sealed class PagedCollection<T> : IPagedCollection<T>
    {
        [NonSerialized]
        private readonly List<T> _list = new List<T>();

        public PagedCollection(IReadOnlyList<T> items, int itemCount, int pageNumber, int pageSize)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (items.Any(item => item == null))
                throw new ArgumentException("Only non-nullable items allowed", (nameof(items)));

            if (itemCount < 0)
                throw new ArgumentOutOfRangeException(nameof(itemCount), "Value must be greater than or equal to zero");

            if (pageNumber < 0)
                throw new ArgumentException("Value must be greater than or equal to zero", nameof(pageNumber));

            if (pageSize < 0)
                throw new ArgumentException("Value must be greater than or equal to zero", nameof(pageSize));

            ItemCount = itemCount;
            CurrentPageNumber = pageNumber;
            PageSize = pageSize;
            PageCount = ComputePageCount(pageSize, itemCount);
            _list.AddRange(items);
        }

        public int CurrentPageNumber { get; }
        public int ItemCount { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public int LastPageNumber => PageCount;
        public int? NextPageNumber => HasNext ? CurrentPageNumber + 1 : default(int?);
        public int? PreviousPageNumber => HasPrevious ? CurrentPageNumber - 1 : default(int?);
        public bool HasPrevious => CurrentPageNumber > 1;
        public bool HasNext => CurrentPageNumber < PageCount;

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        private static int ComputePageCount(int pageSize, int itemCount)
        {
            return pageSize > 0 ? (int)Math.Ceiling(itemCount / (double)pageSize) : 0;
        }
    }
}
