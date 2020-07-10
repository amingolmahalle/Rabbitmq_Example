using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RabbitMq_Common.RabbitMq.Extensions
{
    public static class RabbitMqListenerExtension
    {
        private static IRabbitMqBus Bus { get; set; }

        public static void UseRabbitMqListener(this IApplicationBuilder builder)
        {
            Bus = builder.ApplicationServices.GetService<IRabbitMqBus>();
            var life = builder.ApplicationServices.GetService<IApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);
        }

        private static void OnStarted()
        {
            Bus.Subscribe();
        }

        private static void OnStopping()
        {
            Bus.Dispose();
        }
    }
}