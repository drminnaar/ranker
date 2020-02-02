using System.Collections.Generic;

namespace System.Linq
{
    public static class IReadOnlyListExtensions
    {
        public static PagedCollection<T> ToPagedCollection<T>(
            this IReadOnlyList<T> source,
            int itemCount,
            int pageNumber,
            int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Source of paged collection may not be null");

            if (itemCount < 0)
                throw new ArgumentOutOfRangeException(nameof(itemCount), "Item count must be positive");

            if (pageNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be positive");

            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be positive");

            return new PagedCollection<T>(source, itemCount, pageNumber, pageSize);
        }
    }
}
