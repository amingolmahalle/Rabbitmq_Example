namespace Common.RabbitMq
{
    public interface IRabbitMqBus
    {
        void Send(object message);
    }
}