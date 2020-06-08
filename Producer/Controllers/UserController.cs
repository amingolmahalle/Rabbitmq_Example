using Common.RabbitMq;
using Microsoft.AspNetCore.Mvc;

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
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var message = "helllllllllllllllo";//inja bayad az GetallUserCommand ye new besazi

            _rabbitMqBus.Send(message, "directexchange_key", "testExchange");

            return Ok("Message Sent");
        }
    }
}