using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Microsoft.EntityFrameworkCore
{
    public abstract class EqualityFilterBase<TEntity, TValue> where TValue : struct, IComparable<TValue>
    {
        private IDictionary<string, Expression<Func<TEntity, bool>>> _lookup = null!;
        private readonly string _filterExpression;

        protected EqualityFilterBase(string filterExpression)
        {
            if (string.IsNullOrWhiteSpace(filterExpression))
                throw new ArgumentException("The specified filter expression may not be null, empty, or whitespace", nameof(filterExpression));

            if (!IsValidFilterExpression(filterExpression))
                throw new ArgumentException("Must be in the form of 'gt:10', 'gte:10', 'eq:10'", nameof(filterExpression));

            _filterExpression = filterExpression;
            (EqualitySymbol, Value) = Parse(filterExpression);
        }

        public TValue Value { get; }
        public string EqualitySymbol { get; } = string.Empty;

        public Expression<Func<TEntity, bool>> Expression() => LookupExpression();

        public override string ToString()
        {
            return _filterExpression;
        }

        protected abstract IDictionary<string, Expression<Func<TEntity, bool>>> CreateEqualityOperatorExpressionLookup();

        private Expression<Func<TEntity, bool>> LookupExpression()
        {
            if (_lookup == null)
            {
                _lookup = CreateEqualityOperatorExpressionLookup()
                    ?? throw new InvalidOperationException("Expected non-null equality operator expression lookup");
            }

            if (!_lookup.TryGetValue(EqualitySymbol, out var value))
                throw new NotSupportedException($"The equality symbol '{EqualitySymbol}' is not supported.");

            return value;
        }

        private static bool IsValidFilterExpression(string filterExpression)
        {
            return Regex.IsMatch(filterExpression, @"^(gt|gte|lt|lte|eq):(([1-9]{1}\d*\.?\d*)|([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])))$");
        }

        private static (string equalityOperator, TValue value) Parse(string filterExpression)
        {
            var filterTokens = filterExpression.Split(":");
            return (filterTokens[0].Trim(), filterTokens[1].ConvertTo<TValue>());
        }
    }
}
