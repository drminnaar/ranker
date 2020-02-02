using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.EntityFrameworkCore
{
    public sealed class OrderExpression : OrderExpressionBase
    {
        private const string ValidationPattern = @"^((-)?[a-zA-Z][a-zA-Z]*)(-[a-zA-Z]+)*((,|,\s|\s,|\s,\s)?((-)?[a-zA-Z][a-zA-Z]*)(-[a-zA-Z]+)*)$";

        public OrderExpression(string orderExpression) : base(orderExpression)
        {
        }

        public override bool IsAscending()
        {
            if (!IsValid())
                throw NewInvalidORderExpressionException();

            return !Value.StartsWith("-", true, CultureInfo.InvariantCulture);
        }

        public override bool IsDescending()
        {
            if (!IsValid())
                throw NewInvalidORderExpressionException();

            return Value.StartsWith("-", true, CultureInfo.InvariantCulture);
        }

        public override string GetSelector()
        {
            if (!IsValid())
                throw NewInvalidORderExpressionException();

            return Regex.Replace(Value, "^-+", string.Empty).Replace("+", string.Empty, true, CultureInfo.InvariantCulture);
        }

        public override bool IsValid()
        {
            return Regex.IsMatch(Value, ValidationPattern);
        }

        private InvalidOperationException NewInvalidORderExpressionException()
        {
            return new InvalidOperationException($"The specified order '{Value}' is invalid."
                + " The default sort order is ascending. Use '-' at the start of expression to sort descending."
                + " The sort value may use 'kebab case' AKA 'hyphen case'."
                + " Some examples of valid order expressions are 'name', '-name', 'unit-price', '-unit-price'");
        }
    }
}
