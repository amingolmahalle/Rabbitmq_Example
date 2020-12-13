using RabbitMq_Common.Exception;
using RabbitMq_Common.RabbitMq.DomainEvent;
using RabbitMq_Common.RabbitMq.Event.Enums;

namespace RabbitMq_Common.RabbitMq.Event
{
    public class EventFactory : IEventFactory
    {
        public IEvent CreateEvent(string exchange)
        {
            IEvent ev;
            
            if (exchange == EventType.SendEmail.ToString())
                ev = new SendEmailEvent();

            else if (exchange == EventType.SendSms.ToString())
                ev = new SendSmsEvent();

            else
                throw new EventNotSupportedException(exchange);

            return ev;
        }
    }
}