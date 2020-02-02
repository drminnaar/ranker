using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace System.Linq
{
    public static class IQueryableExtensions
    {
        public static PagedCollection<T> ToPagedCollection<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Source of paged collection may not be null");

            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero");

            var itemCount = source.AsEnumerable().Count();

            var items = source
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            return new PagedCollection<T>(items, itemCount, pageNumber, pageSize);
        }

        public static IQueryable<T> TagWithQueryName<T>(this DbSet<T> source, string queryName) where T : class
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrWhiteSpace(queryName))
                throw new ArgumentException("The specified query name may not be null, empty or whitespace", nameof(queryName));

            return source.TagWith($"QueryName: {queryName}");
        }
    }
}
