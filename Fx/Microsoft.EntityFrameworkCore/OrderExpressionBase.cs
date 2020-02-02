using System.Globalization;

namespace Microsoft.EntityFrameworkCore
{
    public abstract class OrderExpressionBase
    {
        protected OrderExpressionBase(string orderExpression)
        {
            if (string.IsNullOrWhiteSpace(orderExpression))
                throw new System.ArgumentException("A non-empty order expression is required", nameof(orderExpression));

            Value = orderExpression.Trim().ToLower(CultureInfo.CurrentCulture);
        }

        public abstract bool IsAscending();
        public abstract bool IsDescending();
        public string Value { get; }

        public abstract string GetSelector();
        public abstract bool IsValid();
    }
}
