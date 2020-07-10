using System;

namespace RabbitMq_Common.RabbitMq
{
    public interface IRabbitMqBus : IDisposable
    {
        void Send(object message);
        void Subscribe();
    }
}