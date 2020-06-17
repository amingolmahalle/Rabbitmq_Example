using System;
using System.Threading.Tasks;
using Common.RabbitMq;
using Consumer.Contracts.User;
using Consumer.Repositories;

namespace Consumer.Commands.User.GetUserByMobileNumber
{
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