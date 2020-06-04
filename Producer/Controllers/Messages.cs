using Common.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace Producer.Controllers
{
    [Route("messages")]
    public class Messages : Controller
    {
        private readonly IRabbitMqBus _rabbitMqBus;

        public Messages(IRabbitMqBus rabbitMqBus)
        {
            _rabbitMqBus = rabbitMqBus;
        }

        [HttpGet]
        [Route("send")]
        public IActionResult Send()
        {
            //   byte[] messageBuffer = Encoding.Default.GetBytes("Send data by Direct Message");
            var message = "send data for check rabbitMq";

            _rabbitMqBus.Send(message, "directexchange_key", "testExchange");

            return Ok("Message Sent");
        }
    }
}