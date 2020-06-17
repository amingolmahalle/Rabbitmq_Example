using System;
using RabbitMQ.Client;

namespace Common.RabbitMq
{
    public interface IRabbitMqConnection: IDisposable
    {
        IConnection TryConnection();
    }
}