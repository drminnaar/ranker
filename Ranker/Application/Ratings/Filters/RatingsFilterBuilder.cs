using System;
using System.Linq.Expressions;
using Ranker.Domain.Models;

namespace Ranker.Application.Ratings.Filters
{
    public sealed class RatingsFilterBuilder : IRatingsFilterBuilder
    {
        public Expression<Func<Rating, bool>> Filter { get; private set; } = ExpressionBuilder.True<Rating>();

        public IRatingsFilterBuilder WhereUserId(long? userId)
        {
            if (userId.HasValue)
                Filter = Filter.And(rating => rating.UserId == userId.Value);

            return this;
        }

        public IRatingsFilterBuilder WhereMovieId(long? movieId)
        {
            if (movieId.HasValue)
                Filter = Filter.And(rating => rating.MovieId == movieId.Value);

            return this;
        }

        public IRatingsFilterBuilder WhereRating(string? minimumRating, string? maximumRating)
        {
            if (!string.IsNullOrWhiteSpace(minimumRating))
                Filter = Filter.And(new RatingFilter(minimumRating).Expression());

            if (!string.IsNullOrWhiteSpace(maximumRating))
                Filter = Filter.And(new RatingFilter(maximumRating).Expression());

            return this;
        }

        public IRatingsFilterBuilder WhereTimestamp(string? minimumTimestamp, string? maximumTimestamp)
        {
            if (!string.IsNullOrWhiteSpace(minimumTimestamp))
                Filter = Filter.And(new TimestampFilter(minimumTimestamp).Expression());

            if (!string.IsNullOrWhiteSpace(maximumTimestamp))
                Filter = Filter.And(new TimestampFilter(maximumTimestamp).Expression());

            return this;
        }
    }
}
