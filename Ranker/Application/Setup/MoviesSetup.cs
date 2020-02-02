using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ranker.Application.Movies;
using Ranker.Application.Movies.Filters;
using Ranker.Application.Movies.Orders;
using Ranker.Domain.Models;

namespace Ranker.Application.Setup
{
    public static class MoviesSetup
    {
        public static IServiceCollection ConfigureMovies(this IServiceCollection services)
        {
            return services
                .AddScoped<IMovieFilterBuilder, MovieFilterBuilder>()
                .AddScoped<IEntityOrderBuilder<Movie>, MovieOrderBuilder>()
                .AddScoped<IMovieService, MovieService>();
        }
    }
}
