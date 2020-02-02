using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore
{
    public abstract class EntityOrderBuilderBase<T> : IEntityOrderBuilder<T>, IEntitySubsequentOrderBuilder<T>
    {
        private IDictionary<string, Expression<Func<T, object>>> _selectorLookup = null!;
        private IOrderedQueryable<T> _queryable = Enumerable.Empty<T>().AsQueryable().OrderBy(e => 1);

        protected EntityOrderBuilderBase()
        {
        }

        public IOrderedQueryable<T> Expression => _queryable;
        public IOrderedQueryable<T> SubsequentExpression => _queryable;

        public IEntitySubsequentOrderBuilder<T> OrderBy(IQueryable<T> source, string orderExpression)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrWhiteSpace(orderExpression))
            {
                _queryable = source.OrderBy(e => 1);
                return this;
            }

            var orderExpressions = orderExpression
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(e => new OrderExpression(e))
                .Where(e => e.IsValid())
                .ToList();

            return OrderBy(source, orderExpressions);
        }

        public IEntitySubsequentOrderBuilder<T> OrderBy(IQueryable<T> source, IReadOnlyList<OrderExpressionBase> orderExpressions)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (orderExpressions == null)
                throw new ArgumentNullException(nameof(orderExpressions));

            if (orderExpressions.Count == 0)
            {
                _queryable = source.OrderBy(e => 1);
                return this;
            }
            else if (orderExpressions.Count == 1)
            {
                return OrderBy(source, orderExpressions[0]);
            }
            else if (orderExpressions.Count > 1)
            {
                OrderBy(source, orderExpressions[0]);

                for (var i = 1; i < orderExpressions.Count; i++)
                {
                    ThenOrderBy(orderExpressions[i]);
                }
                return this;
            }

            return this;
        }

        public IEntitySubsequentOrderBuilder<T> OrderBy(IQueryable<T> source, OrderExpressionBase orderExpression)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (orderExpression == null)
                throw new ArgumentNullException(nameof(orderExpression));

            var selector = LookupSelector(orderExpression.GetSelector());

            if (selector == null)
            {
                _queryable = source.OrderBy(e => 1);
                return this;
            }

            _queryable = orderExpression.IsDescending()
                ? source.OrderByDescending(selector)
                : source.OrderBy(selector);

            return this;
        }

        public IEntitySubsequentOrderBuilder<T> ThenOrderBy(OrderExpressionBase orderExpression)
        {
            if (_queryable == null || orderExpression == null)
                return this;

            var selectorExpression = LookupSelector(orderExpression.GetSelector());

            if (selectorExpression == null)
                return this;

            _queryable = orderExpression.IsDescending()
                ? _queryable.ThenByDescending(selectorExpression)
                : _queryable.ThenBy(selectorExpression);

            return this;
        }

        protected abstract IDictionary<string, Expression<Func<T, object>>> CreateSelectorLookup();

        private Expression<Func<T, object>>? LookupSelector(string selector)
        {
            if (_selectorLookup == null)
            {
                _selectorLookup = CreateSelectorLookup()
                    ?? throw new InvalidOperationException("Expected non-null expression selector lookup");
            }

            _selectorLookup.TryGetValue(selector, out var selectorExpression);
            return selectorExpression;
        }
    }
}
