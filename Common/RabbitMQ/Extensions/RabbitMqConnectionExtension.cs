using Microsoft.AspNetCore.Builder;

namespace Common.RabbitMQ.Extensions
{
    public static class RabbitMqConnectionExtension
    {
        public static void UseRabbitMqConnectionListener(
            this IApplicationBuilder app,
            IRabbitMqConnection rabbitMqConnection)
        {
            rabbitMqConnection.CreateConnection();
        }
    }
}