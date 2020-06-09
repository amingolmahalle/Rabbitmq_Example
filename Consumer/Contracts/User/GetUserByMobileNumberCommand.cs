using Common.RabbitMq;

namespace Consumer.Contracts.User
{
    public class GetUserByMobileNumberCommand : ICommand
    {
        public string MobileNumber { get; set; }
    }
}