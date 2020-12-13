namespace RabbitMq_Common.RabbitMq.Event
{
    public interface IEventAckNack
    {
        void Ack(ulong deliveryTag);
        
        void Nack(ulong deliveryTag, bool retry);
    }
}