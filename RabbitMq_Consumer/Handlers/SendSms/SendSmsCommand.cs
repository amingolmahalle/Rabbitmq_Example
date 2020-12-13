using RabbitMq_Common.RabbitMq;

namespace RabbitMq_Consumer.Handlers.SendSms
{
    public class SendSmsCommand : IConsumerCommand
    {
        public string Message { get; set; }
        
        public string Mobile { get; set; }
    }
}