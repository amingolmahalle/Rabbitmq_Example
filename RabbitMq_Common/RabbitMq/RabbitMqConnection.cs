using System;
using RabbitMQ.Client;

namespace RabbitMq_Common.RabbitMq
{
    public sealed class RabbitMqConnection
    {
        private const string UserName = SystemConstants.Username;

        private const string Password = SystemConstants.Password;

        private const string HostName = SystemConstants.HostName;

        private IConnection _connection;

        private readonly object _syncRoot = new object();

        private bool IsConnected =>
            _connection != null && _connection.IsOpen;

        public IConnection TryConnection()
        {
            if (!IsConnected)
            {
                lock (_syncRoot)
                {
                    try
                    {
                        var connectionFactory = new ConnectionFactory
                        {
                            HostName = HostName,
                            UserName = UserName,
                            Password = Password
                        };

                        _connection = connectionFactory.CreateConnection();
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"{e} => RabbitMq Connection Failed");
                    }
                }
            }

            return _connection;
        }
    }
}