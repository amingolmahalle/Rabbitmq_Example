namespace Common.RabbitMq.Routing
{
    public class MessageRouteTableValue
    {
        public string Namespace { get; set; }
        
        public string Consumer { get; set; }
        
        public MessageType Type { get; set; }
    }
}