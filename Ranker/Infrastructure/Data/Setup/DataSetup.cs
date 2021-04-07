using System;
using Ranker.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Infrastructure.Data.Setup
{
    public static class DataSetup
    {
        public static IServiceCollection ConfigureData(this IServiceCollection services, bool isDevelopment)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            return services
                .AddDbContextPool<RatingsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("ranker");
                    options.EnableDetailedErrors(isDevelopment);
                    options.EnableSensitiveDataLogging(isDevelopment);
                })
                .AddScoped<Seeder>()
                .AddScoped<IRatingsDbContext>(provider => provider.GetRequiredService<RatingsDbContext>());
        }
    }
}
