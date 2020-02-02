using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application.Users.Orders
{
    public sealed class UserOrderBuilder : EntityOrderBuilderBase<User>
    {
        public UserOrderBuilder() : base()
        {
        }

        protected override IDictionary<string, Expression<Func<User, object>>> CreateSelectorLookup()
        {
            return new Dictionary<string, Expression<Func<User, object>>>
            {
                { "age", e => e.Age! },
                { "email", e => e.Email! },
                { "first-name", e => e.FirstName! },
                { "gender", e => e.UserId! },
                { "last-name", e => e.LastName! },
                { "id", e => e.UserId! }
            };
        }
    }
}
