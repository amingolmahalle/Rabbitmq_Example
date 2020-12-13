using System.Collections.Generic;
using RabbitMq_Common.RabbitMq.Event;
using RabbitMq_Common.RabbitMq.Event.Enums;

namespace RabbitMq_Common.RabbitMq.DomainEvent
{
    public abstract class BaseEvent : IEvent
    {
        protected BaseEvent()
        {
            Context = new Dictionary<string, object>();
        }
        
        public abstract EventType EventType { get; set; }
        
        public Dictionary<string, object> Context { get; set; }

        public string Payload { get; set; }

        public string RoutingKey { get; set; }
        
        public string EndpointId { get; set; }
    }
}