using RabbitMq_Common.RabbitMq;

namespace RabbitMq_Producer.Commands
{
    public class SendEmailCommand : IProducerCommand
    {
        public string Email { get; set; }
        
        public string Subject { get; set; }
        
        public string Message { get; set; }
    }
}