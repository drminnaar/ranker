using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ranker.Domain.Models;

namespace Ranker.Application.Movies.Filters
{
    public interface IMovieFilterBuilder
    {
        Expression<Func<Movie, bool>> Filter { get; }

        IMovieFilterBuilder WhereGenresEquals(IReadOnlyCollection<string>? genres);
        IMovieFilterBuilder WhereTitleEquals(string? title);
    }
}