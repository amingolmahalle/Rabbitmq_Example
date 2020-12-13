using RabbitMq_Common.RabbitMq.Event.Enums;

namespace RabbitMq_Common.RabbitMq.DomainEvent
{
    public class SendSmsEvent : BaseEvent
    {
        public SendSmsEvent()
        {
            EventType = EventType.SendSms;
        }
        
        public sealed override EventType EventType { get; set; }
    }
}