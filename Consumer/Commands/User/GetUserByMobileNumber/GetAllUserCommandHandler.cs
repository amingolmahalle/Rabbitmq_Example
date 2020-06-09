using System.Threading.Tasks;
using Common.Attributes;
using Common.RabbitMq;
using Consumer.Contracts.User;
using Consumer.Repositories;

namespace Consumer.Commands.User.GetUserByMobileNumber
{
    [Queue(queueName: "testQueue", exchangeName: "testExchange", routingKey: "directExchange_key")]
    public class GetUserByMobileNumberCommandHandler : IHandleCommand<GetUserByMobileNumberCommand>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByMobileNumberCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(GetUserByMobileNumberCommand message)
        {
            var result = await _userRepository.GetUserByMobileNumberAsync(message.MobileNumber);
            
            // TODO: Reply Result To Publisher 
        }
    }
}