using Microsoft.Extensions.DependencyInjection;
using RabbitMq_Consumer.Handlers.SendEmail;
using RabbitMq_Consumer.Handlers.SendSms;

namespace RabbitMq_Consumer.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<SendEmailCommandHandler>();
            services.AddSingleton<SendSmsCommandHandler>();
        }
    }
}