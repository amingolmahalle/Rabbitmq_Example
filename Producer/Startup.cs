using Common.Extensions;
using Common.RabbitMq;
using Common.RabbitMq.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Producer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddServiceBus(SystemConstants.HostEndpointId,SystemConstants.HostEndpointName);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseRabbitMqConnectionListener(new RabbitMqConnection());
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}