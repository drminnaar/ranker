using System;
using System.Linq.Expressions;
using Ranker.Domain.Models;

namespace Ranker.Application.Users.Filters
{
    public interface IUserFilterBuilder
    {
        Expression<Func<User, bool>> Filter { get; }

        IUserFilterBuilder WhereAge(string? minimumAge, string? maximumAge);
        IUserFilterBuilder WhereEmailEquals(string? email);
        IUserFilterBuilder WhereFirstNameEquals(string? firstName);
        IUserFilterBuilder WhereGenderEquals(string? gender);
        IUserFilterBuilder WhereLastNameEquals(string? lastName);
    }
}
