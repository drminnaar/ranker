using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ranker.Api.Setup
{
    public static class ApiSetup
    {
        public static IServiceCollection ConfigureApi(this IServiceCollection services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            return services
                .AddResponseCaching()
                .ConfigureResources()
                .ConfigureUrlHelper()
                .ConfigureControllers()
                .ConfigureSwagger();
        }
    }
}
