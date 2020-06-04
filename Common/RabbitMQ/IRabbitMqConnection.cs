using RabbitMQ.Client;

namespace Common.RabbitMQ
{
    public interface IRabbitMqConnection
    {
        IModel GetConnection();
        void CreateConnection();
    }
}