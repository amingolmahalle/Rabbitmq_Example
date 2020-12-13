using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMq_Common.Extension;
using RabbitMq_Common.RabbitMq.Event;
using RabbitMq_Consumer.Extension;
using RabbitMq_Consumer.Handlers.SendEmail;
using RabbitMq_Consumer.Handlers.SendSms;

namespace RabbitMq_Consumer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRabbitMqServices();
            services.AddCustomServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });

            InitializeRabbitMqListener(app);
        }

        private void InitializeRabbitMqListener(IApplicationBuilder app)
        {
            var eventConsumerService = app.ApplicationServices.GetService<IRabbitConnector>();

            eventConsumerService
                .InitializeAsync(SystemConstants.ServiceName)
                .ContinueWith(t =>
                {
                    eventConsumerService
                        .ConsumeAsync(SystemConstants.HostEndpointName)
                        .GetAwaiter()
                        .GetResult();
                });

            var sendEmailCommandHandler = app.ApplicationServices.GetService<SendEmailCommandHandler>();

            var sendSmsCommandHandler = app.ApplicationServices.GetService<SendSmsCommandHandler>();

            var dispatcher = app.ApplicationServices.GetService<IJobDispatcher>();

            dispatcher.RegisterHandler("SendEmail", sendEmailCommandHandler);
            dispatcher.RegisterHandler("SendSms", sendSmsCommandHandler);
        }
    }
}