using System;
using RabbitMQ.Client;

namespace Common.RabbitMq
{
    public sealed class RabbitMqConnection : IRabbitMqConnection
    {
        private const string UserName = "guest";

        private const string Password = "guest";

        private const string HostName = "localhost";

        private IConnection _connection;

        private bool _disposed;

        private bool IsConnected
        {
            get
            {
                if (_connection != null && _connection.IsOpen && !_disposed)
                {
                    return true;
                }
                
                return false;
            }
        }

        public bool TryConnection()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            _connection = connectionFactory.CreateConnection();

            return IsConnected;
            // var channel = connection.CreateModel();
            //
            // // Success Creating Exchange 
            // channel.ExchangeDeclare("testExchange", ExchangeType.Direct);
            //
            // // Success Creating Queue
            // channel.QueueDeclare("testQueue", true, false, false, null);
            //
            // // Success Creating Binding
            // channel.QueueBind("testQueue", "testExchange", "directexchange_key");
        }

        public IModel CreateModel()
        {
            if (IsConnected)
            {
                var channel = _connection.CreateModel();
                channel.BasicQos(0, 1, false);

                return channel;
            }
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _connection.Dispose();
        }
    }
}