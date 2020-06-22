using RabbitMq_Common.RabbitMq;

namespace RabbitMq_Consumer.Contracts.Users
{
    public class GetUserByMobileNumberCommand : ICommand
    {
        public string MobileNumber { get; set; }
    }
}