using System.Collections.Generic;
using RabbitMQ.Client;

namespace Common.RabbitMq
{
    public interface IRabbitMqBus
    {
        void Send(object message, string routingKey, string exchange);

        void Receive(string queueName, IBasicConsumer consumer, string consumerTag = "",
            IDictionary<string, object> arguments = null);
    }
}