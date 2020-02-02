using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application.Users.Filters
{
    public sealed class AgeFilter : EqualityFilterBase<User, int>
    {
        public AgeFilter(string filterExpression) : base(filterExpression)
        {
        }

        protected override IDictionary<string, Expression<Func<User, bool>>> CreateEqualityOperatorExpressionLookup()
        {
            return new Dictionary<string, Expression<Func<User, bool>>>
            {
                { "gt", e => e.Age > Value},
                { "gte", e => e.Age >= Value},
                { "lt", e => e.Age < Value},
                { "lte", e => e.Age <= Value},
                { "eq", e => e.Age == Value}
            };
        }
    }
}
