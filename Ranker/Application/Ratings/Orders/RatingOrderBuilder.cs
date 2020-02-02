using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application.Ratings.Orders
{
    public sealed class RatingOrderBuilder : EntityOrderBuilderBase<Rating>
    {
        protected override IDictionary<string, Expression<Func<Rating, object>>> CreateSelectorLookup()
        {
            return new Dictionary<string, Expression<Func<Rating, object>>>
            {
                { "ratingid", e => e.RatingId! },
                { "userid", e => e.UserId! },
                { "movieid", e => e.MovieId! },
                { "score", e => e.Score! },
                { "timestamp", e => e.Timestamp! }
            };
        }
    }
}
