using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application.Movies.Filters
{
    public sealed class MovieFilterBuilder : IMovieFilterBuilder
    {
        public Expression<Func<Movie, bool>> Filter { get; private set; } = ExpressionBuilder.True<Movie>();

        public IMovieFilterBuilder WhereTitleEquals(string? title)
        {
            if (!string.IsNullOrWhiteSpace(title))
                Filter = Filter.And(movie => movie.Title.ToLower() == title.Trim().ToLower());

            return this;
        }

        public IMovieFilterBuilder WhereGenresEquals(IReadOnlyCollection<string>? genres)
        {
            if (genres != null)
            {
                foreach (var genre in genres)
                {
                    //Filter = Filter.And(movie => EF.Functions.ILike(movie.Genres.ToLower(), $"%{genre.Trim().ToLower()}%"));
                    Filter = Filter.And(movie => movie.Genres.ToLower().Contains(genre.Trim().ToLower()));
                }
            }

            return this;
        }
    }
}
