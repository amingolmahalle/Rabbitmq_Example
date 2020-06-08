using System;
using System.Threading.Tasks;
using Common.Attributes;
using Common.RabbitMq;

namespace Consumer.Services.Commands.GetAllUser
{
    [Queue(queueName: "testQueue", exchangeName: "testExchange", routingKey: "directExchange_key")]
    public class GetAllUserCommandHandler : IHandleCommand<GetAllUserCommand>
    {
        public async Task Handle(GetAllUserCommand message)
        {
            Console.WriteLine($@"{nameof(message.Id)}= {message.Id},
                                 {nameof(message.MobileNumber)}= {message.MobileNumber},
                                 {nameof(message.BirthDate)}= {message.BirthDate},
                                 {nameof(message.Email)}= {message.Email},
                                 {nameof(message.IsActive)}= {message.IsActive},
                                 {nameof(message.Fullname)}= {message.Fullname}");
            
        }
    }
}