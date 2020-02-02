using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application.Ratings.Filters
{
    public sealed class RatingFilter : EqualityFilterBase<Rating, double>
    {
        public RatingFilter(string filterExpression) : base(filterExpression)
        {
        }

        protected override IDictionary<string, Expression<Func<Rating, bool>>> CreateEqualityOperatorExpressionLookup()
        {
            return new Dictionary<string, Expression<Func<Rating, bool>>>
            {
                { "gt", e => e.Score > Value},
                { "gte", e => e.Score >= Value},
                { "lt", e => e.Score < Value},
                { "lte", e => e.Score <= Value},
                { "eq", e => e.Score == Value}
            };
        }
    }
}
