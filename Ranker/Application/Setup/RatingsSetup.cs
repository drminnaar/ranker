using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ranker.Application.Ratings;
using Ranker.Application.Ratings.Filters;
using Ranker.Application.Ratings.Orders;
using Ranker.Domain.Models;

namespace Ranker.Application.Setup
{
    public static class RatingsSetup
    {
        public static IServiceCollection ConfigureRatings(this IServiceCollection services)
        {
            return services
                .AddScoped<IRatingsFilterBuilder, RatingsFilterBuilder>()
                .AddScoped<IEntityOrderBuilder<Rating>, RatingOrderBuilder>()
                .AddScoped<IRatingService, RatingService>();
        }
    }
}
