using RabbitMq_Common.RabbitMq.Event.Enums;

namespace RabbitMq_Common.RabbitMq.DomainEvent
{
    public class SendEmailEvent : BaseEvent
    {
        public SendEmailEvent()
        {
            EventType = EventType.SendEmail;
        }

        public sealed override EventType EventType { get; set; }
    }
}