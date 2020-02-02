using System;
using System.Linq.Expressions;
using Ranker.Domain.Models;

namespace Ranker.Application.Ratings.Filters
{
    public interface IRatingsFilterBuilder
    {
        Expression<Func<Rating, bool>> Filter { get; }

        IRatingsFilterBuilder WhereMovieId(long? movieId);
        IRatingsFilterBuilder WhereRating(string? minimumRating, string? maximumRating);
        IRatingsFilterBuilder WhereTimestamp(string? minimumTimestamp, string? maximumTimestamp);
        IRatingsFilterBuilder WhereUserId(long? userId);
    }
}
