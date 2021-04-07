using System;
using System.Linq.Expressions;
using Ranker.Domain.Models;

namespace Ranker.Application.Users.Filters
{
    public sealed class UserFilterBuilder : IUserFilterBuilder
    {
        public Expression<Func<User, bool>> Filter { get; private set; } = ExpressionBuilder.True<User>();

        public IUserFilterBuilder WhereAge(string? minimumAge, string? maximumAge)
        {
            if (!string.IsNullOrWhiteSpace(minimumAge))
                Filter = Filter.And(new AgeFilter(minimumAge).Expression());

            if (!string.IsNullOrWhiteSpace(maximumAge))
                Filter = Filter.And(new AgeFilter(maximumAge).Expression());

            return this;
        }

        public IUserFilterBuilder WhereEmailEquals(string? email)
        {
            if (!string.IsNullOrWhiteSpace(email))
                Filter = Filter.And(user => user.Email.ToUpperInvariant() == email.Trim().ToUpperInvariant());

            return this;
        }

        public IUserFilterBuilder WhereGenderEquals(string? gender)
        {
            if (!string.IsNullOrWhiteSpace(gender))
                Filter = Filter.And(user => user.Gender.ToUpperInvariant() == gender.Trim().ToUpperInvariant());

            return this;
        }

        public IUserFilterBuilder WhereFirstNameEquals(string? firstName)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                Filter = Filter.And(user => user.FirstName.ToUpperInvariant() == firstName.Trim().ToUpperInvariant());

            return this;
        }

        public IUserFilterBuilder WhereLastNameEquals(string? lastName)
        {
            if (!string.IsNullOrWhiteSpace(lastName))
                Filter = Filter.And(user => user.LastName.ToUpperInvariant() == lastName.Trim().ToUpperInvariant());

            return this;
        }
    }
}
