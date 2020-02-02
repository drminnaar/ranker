using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    public interface IEntityOrderBuilder<T>
    {
        IOrderedQueryable<T> Expression { get; }
        IEntitySubsequentOrderBuilder<T> OrderBy(IQueryable<T> source, string orderExpression);
        IEntitySubsequentOrderBuilder<T> OrderBy(IQueryable<T> source, OrderExpressionBase orderExpression);
        IEntitySubsequentOrderBuilder<T> OrderBy(IQueryable<T> source, IReadOnlyList<OrderExpressionBase> orderExpressions);
    }
}