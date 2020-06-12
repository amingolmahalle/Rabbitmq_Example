using System;
using System.Threading.Tasks;
using Common.Attributes;
using Common.RabbitMq;
using Consumer.Contracts.User;
using Consumer.Repositories;

namespace Consumer.Commands.User.GetUserByMobileNumber
{
    [Queue(queueName: "Consumer.Host", exchangeName: "testExchange", routingKey: "directExchange_key")]
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
            
            Console.WriteLine("receive data successfully!");
            // TODO: Reply Result To Publisher 
        }
    }
}