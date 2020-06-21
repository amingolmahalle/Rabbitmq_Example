using System;
using System.Threading.Tasks;
using RabbitMq_Common.RabbitMq;
using RabbitMq_Consumer.Repositories;

namespace RabbitMq_Consumer.Commands.User.GetUserByMobileNumberCommand
{
    public class GetUserByMobileNumberCommandHandler : IHandleCommand<Contracts.User.GetUserByMobileNumberCommand>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByMobileNumberCommandHandler(IUserRepository userRepository)
        {
           _userRepository = userRepository;
        }

        public async Task Handle(Contracts.User.GetUserByMobileNumberCommand message)
        {
            var result = await _userRepository.GetUserByMobileNumberAsync(message.MobileNumber);
            
            Console.WriteLine("receive data successfully!");
            // TODO: Reply Result To Publisher 
        }
    }
}