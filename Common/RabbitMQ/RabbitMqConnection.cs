using RabbitMQ.Client;

namespace Common.RabbitMQ
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private const string UserName = "guest";

        private const string Password = "guest";

        private const string HostName = "localhost";

        public IModel GetConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password,
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.BasicQos(0, 1, false);

            return channel;
        }

        public void CreateConnection()
        {
            const string userName = "guest";
            const string password = "guest";
            const string hostName = "localhost";

            var connectionFactory = new ConnectionFactory()
            {
                UserName = userName,
                Password = password,
                HostName = hostName,
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            // Success Creating Exchange 
            channel.ExchangeDeclare("testExchange", ExchangeType.Direct);

            // Success Creating Queue
            channel.QueueDeclare("testQueue", true, false, false, null);

            // Success Creating Binding
            channel.QueueBind("testQueue", "testExchange", "directexchange_key");
        }
    }
}