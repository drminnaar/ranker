using System;
using System.IO;
using System.Reflection;
using Ranker.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ranker.Api
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            using var host = CreateHostBuilder(args);
            SeedDb(host);
            host.Run();
        }

        internal static IHost CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).Build();
        }

        private static void SeedDb(IHost host)
        {
            var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();
            // TODO: Uncomment the following code to limit database seeding to development
            // var environment = scope.ServiceProvider.GetService<IHostEnvironment>();
            // if (!environment.IsDevelopment())
            //    return;

            var executingPath = Path
                .GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                ?? throw new InvalidOperationException("The current Api execution path may not be null");

            var moviesFilePath = Path.Combine(executingPath, "SeedData/movies.json");
            var ratingsFilePath = Path.Combine(executingPath, "SeedData/ratings.json");
            var tagsFilePath = Path.Combine(executingPath, "SeedData/tags.json");
            var usersFilePath = Path.Combine(executingPath, "SeedData/users.json");

            var seeder = scope
               .ServiceProvider
               .GetRequiredService<Seeder>()
               .IncludeMovies(moviesFilePath)
               .IncludeUsers(usersFilePath)
               .IncludeTags(tagsFilePath)
               .IncludeRatings(ratingsFilePath);

            seeder.Seed();
        }
    }
}
