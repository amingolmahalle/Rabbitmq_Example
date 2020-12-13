using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMq_Common.Extension;
using RabbitMq_Common.RabbitMq.Event;

namespace RabbitMq_Producer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRabbitMqServices();
            
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

            InitStartupServices(app);
        }

        private void InitStartupServices(IApplicationBuilder app)
        {
            IRabbitConnector eventProducerService = app.ApplicationServices.GetService<IRabbitConnector>();

            eventProducerService.InitializeAsync(SystemConstants.ServiceName);
        }
    }
}