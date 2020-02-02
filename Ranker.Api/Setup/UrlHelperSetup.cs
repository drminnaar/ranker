using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Ranker.Api.Setup
{
    internal static class UrlHelperSetup
    {
        internal static IServiceCollection ConfigureUrlHelper(this IServiceCollection services)
        {
            return services
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped(provider =>
                {
                    var actionContext = provider.GetRequiredService<IActionContextAccessor>().ActionContext;
                    var factory = provider.GetRequiredService<IUrlHelperFactory>();
                    return factory.GetUrlHelper(actionContext);
                });
        }
    }
}
