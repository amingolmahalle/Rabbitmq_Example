using Microsoft.AspNetCore.Mvc;
using RabbitMq_Common.RabbitMq;
using RabbitMq_Consumer.Contracts.User;

namespace RabbitMq_Producer.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IRabbitMqBus _rabbitMqBus;

        public UserController(IRabbitMqBus rabbitMqBus)
        {
            _rabbitMqBus = rabbitMqBus;
        }

        [HttpGet]
        [Route("GetUserByMobile/{mobileNumber}")]
        public IActionResult GetUserByMobileNumber([FromRoute] string mobileNumber)
        {
            var message = new GetUserByMobileNumberCommand
            {
                MobileNumber = mobileNumber
            };

            _rabbitMqBus.Send(message);

            return Ok("Message Sent");
        }
    }
}