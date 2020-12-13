using System.Collections.Generic;
using RabbitMq_Common.RabbitMq.Event.Enums;

namespace RabbitMq_Common.RabbitMq.Event
{
    public interface IEvent
    {
        Dictionary<string, object> Context { set; get; }

        string Payload { set; get; }

        string RoutingKey { set; get; }

        string EndpointId { get; set; }
        
        EventType EventType { set; get; }
    }
}