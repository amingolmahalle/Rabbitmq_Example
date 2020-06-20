namespace RabbitMq_Common.RabbitMq
{
    public interface IRabbitMqBus
    {
        void Send(object message);
    }
}