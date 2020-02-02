using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    public interface IEntitySubsequentOrderBuilder<T>
    {
        IOrderedQueryable<T> SubsequentExpression { get; }
        IEntitySubsequentOrderBuilder<T> ThenOrderBy(OrderExpressionBase orderExpression);
    }
}
