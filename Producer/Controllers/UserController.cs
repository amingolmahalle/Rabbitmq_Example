using Common.RabbitMq;
using Microsoft.AspNetCore.Mvc;
using Producer.ViewModels.User;

namespace Producer.Controllers
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
            var message = new GetUserByMobileNumberRequest
            {
                MobileNumber = mobileNumber
            };

            _rabbitMqBus.Send(message);

            return Ok("Message Sent");
        }
    }
}