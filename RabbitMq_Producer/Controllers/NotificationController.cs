using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMq_Common.Extension;
using RabbitMq_Common.RabbitMq.DomainEvent;
using RabbitMq_Common.RabbitMq.Event;
using RabbitMq_Producer.Commands;
using RabbitMq_Producer.Dto;

namespace RabbitMq_Producer.Controllers
{
    [Route("Notification")]
    public class NotificationController : Controller
    {
        private readonly IRabbitConnector _rabbitConnector;

        public NotificationController(IRabbitConnector rabbitConnector)
        {
            _rabbitConnector = rabbitConnector;
        }

        [HttpPost("SendEmail")]
        public async Task SendEmail([FromBody] SendEmailRequest request)
        {
            var message = new SendEmailCommand
            {
                Subject = request.Subject,
                Message = request.Message,
                Email = request.Email
            };

            await _rabbitConnector.RaiseEventAsync(new SendEmailEvent
            {
                Payload = message.ObjectSerializer(),
                RoutingKey = "RabbitMq_Consumer",
                EndpointId = SystemConstants.HostEndpointId
            });
        }

        [HttpPost("SendSms")]
        public async Task SendSms([FromBody] SendSmsRequest request)
        {
            var message = new SendSmsCommand
            {
                Message = request.Message,
                Mobile = request.Mobile
            };

            await _rabbitConnector.RaiseEventAsync(new SendSmsEvent
            {
                Payload = message.ObjectSerializer(),
                RoutingKey = "RabbitMq_Consumer",
                EndpointId = SystemConstants.HostEndpointId
            });
        }
    }
}