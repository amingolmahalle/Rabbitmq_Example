using RabbitMq_Common.RabbitMq;

namespace RabbitMq_Consumer.Handlers.SendEmail
{
    public class SendEmailCommand : IConsumerCommand
    {
        public string Subject { get; set; }
        
        public string Message { get; set; }
        
        public string Email { get; set; }
    }
}