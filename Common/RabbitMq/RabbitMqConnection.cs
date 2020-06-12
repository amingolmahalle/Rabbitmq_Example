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
        
        private object _syncRoot = new object();

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

        public void TryConnection()
        {
            if (!IsConnected)
            {
                lock (_syncRoot)
                {
                    var connectionFactory = new ConnectionFactory
                    {
                        HostName = HostName,
                        UserName = UserName,
                        Password = Password
                    };

                    _connection = connectionFactory.CreateConnection();
                }
            }
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