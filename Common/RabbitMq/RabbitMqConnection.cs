using RabbitMQ.Client;

namespace Common.RabbitMq
{
    public sealed class RabbitMqConnection
    {
        private const string UserName = "guest";

        private const string Password = "guest";

        private const string HostName = "localhost";

        private IConnection _connection;

        private bool _disposed;

        private readonly object _syncRoot = new object();

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

        public IConnection TryConnection()
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

            return _connection;
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