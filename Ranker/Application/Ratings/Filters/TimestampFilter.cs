using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application.Ratings.Filters
{
    public sealed class TimestampFilter : EqualityFilterBase<Rating, long>
    {
        public TimestampFilter(string filterExpression) : base(filterExpression)
        {
        }

        protected override IDictionary<string, Expression<Func<Rating, bool>>> CreateEqualityOperatorExpressionLookup()
        {
            return new Dictionary<string, Expression<Func<Rating, bool>>>
            {
                { "gt", e => e.Timestamp > Value},
                { "gte", e => e.Timestamp >= Value},
                { "lt", e => e.Timestamp < Value},
                { "lte", e => e.Timestamp <= Value},
                { "eq", e => e.Timestamp == Value}
            };
        }
    }
}
