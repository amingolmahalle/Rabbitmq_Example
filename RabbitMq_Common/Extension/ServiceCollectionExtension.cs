using Microsoft.Extensions.DependencyInjection;
using RabbitMq_Common.RabbitMq;
using RabbitMq_Common.RabbitMq.Event;

namespace RabbitMq_Common.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddRabbitMqServices(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitConnector, RabbitConnector>();

            services.AddSingleton<IJobDispatcher, JobDispatcher>();
            
            services.AddSingleton<IEventFactory, EventFactory>();
        }
    }
}