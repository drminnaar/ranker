using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class IQueryableExtensions
    {
        public static Task<IPagedCollection<T>> ToPagedCollectionAsync<T>(
            this IQueryable<T> source,
            int pageNumber,
            int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Value may not be null");

            if (pageNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Value must be greater than or equal to zero");

            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Value must be greater than or equal to zero");

            async Task<IPagedCollection<T>> ToPagedCollectionAsync()
            {
                var itemCount = await source.CountAsync();

                var items = await source
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedCollection<T>(items, itemCount, pageNumber, pageSize);
            }

            return ToPagedCollectionAsync();
        }

        public static IOrderedQueryable<TEntity> OrderBy<TEntity, TOrderBuilder>(
            this IQueryable<TEntity> source,
            string? orderExpression,
            TOrderBuilder orderBuilder) where TOrderBuilder : IEntityOrderBuilder<TEntity>
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (orderBuilder == null)
                throw new ArgumentNullException(nameof(orderBuilder));

            if (string.IsNullOrWhiteSpace(orderExpression))
                return source.OrderBy(e => 1);

            return orderBuilder.OrderBy(source, orderExpression).SubsequentExpression;
        }
    }
}
