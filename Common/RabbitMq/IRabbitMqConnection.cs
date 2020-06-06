using System;
using RabbitMQ.Client;

namespace Common.RabbitMq
{
    public interface IRabbitMqConnection: IDisposable
    {
        bool TryConnection();
        IModel CreateModel();
    }
}