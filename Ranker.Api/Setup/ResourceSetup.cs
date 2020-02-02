using System;
using Microsoft.Extensions.DependencyInjection;
using Ranker.Api.Models.Users;

namespace Ranker.Api.Setup
{
    public static class ResourceSetup
    {
        public static IServiceCollection ConfigureResources(this IServiceCollection services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            return services
                .AddScoped<UserResourceFactory>();
        }
    }
}
