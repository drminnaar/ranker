using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application.Movies.Orders
{
    public class MovieOrderBuilder : EntityOrderBuilderBase<Movie>
    {
        public MovieOrderBuilder() : base()
        {
        }

        protected override IDictionary<string, Expression<Func<Movie, object>>> CreateSelectorLookup()
        {
            return new Dictionary<string, Expression<Func<Movie, object>>>
            {
                { "title", e => e.Title! },
                { "id", e => e.MovieId! }
            };
        }
    }
}
