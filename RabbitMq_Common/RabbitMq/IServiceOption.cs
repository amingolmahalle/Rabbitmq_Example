namespace RabbitMq_Common.RabbitMq
{
    public interface IServiceOption
    {
        string CorrelationId { get; set; }
        
        string Target { get; set; }
        
        string BodyType { get; set; }
    }
}