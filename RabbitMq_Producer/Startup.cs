using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMq_Common.RabbitMq.Extensions;

namespace RabbitMq_Producer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .UseRouteProvider(r => { r.AddRouteFromConfigFile(); })
                .AddServiceBus(SystemConstants.HostEndpointId, SystemConstants.HostEndpointName);

            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}