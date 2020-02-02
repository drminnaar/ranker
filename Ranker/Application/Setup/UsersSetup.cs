using Fx.Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ranker.Application.Users;
using Ranker.Application.Users.Filters;
using Ranker.Application.Users.Orders;
using Ranker.Domain.Models;

namespace Ranker.Application.Setup
{
    public static class UsersSetup
    {
        public static IServiceCollection ConfigureUsers(this IServiceCollection services)
        {
            return services
                .AddScoped<ETagProvider>()
                .AddScoped<IUserFilterBuilder, UserFilterBuilder>()
                .AddScoped<IEntityOrderBuilder<User>, UserOrderBuilder>()
                .AddScoped<IUserService, UserService>();
        }
    }
}
