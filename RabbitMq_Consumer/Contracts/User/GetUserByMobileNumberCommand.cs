using RabbitMq_Common.RabbitMq;

namespace RabbitMq_Consumer.Contracts.User
{
    public class GetUserByMobileNumberCommand : ICommand
    {
        public string MobileNumber { get; set; }
    }
}