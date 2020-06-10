using System;
using RabbitMQ.Client;

namespace Common.RabbitMq
{
    public interface IRabbitMqConnection: IDisposable
    {
        void TryConnection();
        IModel CreateModel();
    }
}