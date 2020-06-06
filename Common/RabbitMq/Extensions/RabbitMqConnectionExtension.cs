using Microsoft.AspNetCore.Builder;

namespace Common.RabbitMq.Extensions
{
    public static class RabbitMqConnectionExtension
    {
        public static void UseRabbitMqConnectionListener(
            this IApplicationBuilder app,
            IRabbitMqConnection rabbitMqConnection)
        {
            rabbitMqConnection.TryConnection();
        }
    }
}