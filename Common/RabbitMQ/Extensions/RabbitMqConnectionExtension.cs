using Microsoft.AspNetCore.Builder;

namespace Common.RabbitMQ.Extensions
{
    public static class RabbitMqConnectionExtension
    {
        public static void UseRabbitMqListener(this IApplicationBuilder app, IRabbitMqConnection rabbitMqConnection)
        {
            rabbitMqConnection.CreateConnection();
        }
    }
}