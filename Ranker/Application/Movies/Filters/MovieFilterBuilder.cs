using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Ranker.Domain.Models;

namespace Ranker.Application.Movies.Filters
{
    public sealed class MovieFilterBuilder : IMovieFilterBuilder
    {
        public Expression<Func<Movie, bool>> Filter { get; private set; } = ExpressionBuilder.True<Movie>();

        public IMovieFilterBuilder WhereTitleEquals(string? title)
        {
            if (!string.IsNullOrWhiteSpace(title))
                Filter = Filter.And(movie => movie.Title!.ToUpperInvariant() == title.Trim().ToUpperInvariant());

            return this;
        }

        public IMovieFilterBuilder WhereGenresEquals(IReadOnlyCollection<string>? genres)
        {
            if (genres != null)
            {
                foreach (var genre in genres)
                {
                    //Filter = Filter.And(movie => EF.Functions.ILike(movie.Genres.ToLower(), $"%{genre.Trim().ToLower()}%"));
                    Filter = Filter.And(movie => movie
                        .Genres
                        .ToUpperInvariant()
                        .Contains(
                            genre.Trim().ToUpperInvariant(),
                            StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return this;
        }
    }
}
