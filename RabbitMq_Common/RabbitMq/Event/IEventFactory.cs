namespace RabbitMq_Common.RabbitMq.Event
{
    public interface IEventFactory
    {
        IEvent CreateEvent(string exchange);
    }
}