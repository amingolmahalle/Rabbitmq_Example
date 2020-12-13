using RabbitMq_Common.RabbitMq;

namespace RabbitMq_Producer.Commands
{
    public class SendSmsCommand : IProducerCommand
    {
        public string Message { get; set; }

        public string Mobile { get; set; }
    }
}