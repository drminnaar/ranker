using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Ranker.Application.Setup
{
    public static class ApplicationSetup
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services)
        {
            return services
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .ConfigureMovies()
                .ConfigureRatings()
                .ConfigureUsers();
        }
    }
}
