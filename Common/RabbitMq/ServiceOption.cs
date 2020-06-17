namespace Common.RabbitMq
{
    public class ServiceOption : IServiceOption
    {
        public string CorrelationId { get; set; }
        
        public string Target { get; set; }
        
        public string BodyType { get; set; }
    }
}