using System;

namespace Common.RabbitMq
{
    public interface IRabbitMqBus
    {
        void Send(object message);

        void Receive(Type @event);
    }
}