using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ranker.Api.Setup;
using Ranker.Application.Setup;
using Ranker.Infrastructure.Data.Setup;

namespace Ranker.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureData(_environment.IsDevelopment());
            services.ConfigureApplication();
            services.ConfigureApi();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler("/errors");
            app.UseStaticFiles();
            app.UseCustomSwagger();
            app.UseRouting();
            app.UseAuthorization();
            app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
