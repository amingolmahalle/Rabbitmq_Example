using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMq_Common.Exception;
using RabbitMq_Common.Extension;
using RabbitMq_Common.RabbitMq;
using RabbitMq_Common.RabbitMq.Event;

namespace RabbitMq_Consumer.Handlers.SendEmail
{
    public class SendEmailCommandHandler : IHandleCommand
    {
        private readonly ILogger<SendEmailCommandHandler> _logger;

        public SendEmailCommandHandler(ILogger<SendEmailCommandHandler> logger)
        {
            _logger = logger;
        }

        public bool CanHandle(IEvent message)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(message.EndpointId))
            {
                _logger?.LogError(
                    $"empty EndPointId received from {message.RoutingKey}, can not process nack/no-retry");

                isValid = false;
            }

            else if (message.EndpointId != SystemConstants.HostEndpointId)
            {
                _logger?.LogError(
                    $"EndPointId Invalid, can not process nack/no-retry");

                isValid = false;
            }

            return isValid;
        }

        public async Task HandleAsync(IEvent message, ulong deliveryTag, IEventAckNack eventAckNack)
        {
            SendEmailCommand command;
            try
            {
                command = message.Payload.ObjectDeserializer<SendEmailCommand>();
            }
            catch (Exception ex)
            {
                _logger?.LogError("could not deserialize payload");
                _logger?.LogCritical(ex, ex.Message);

                throw new HandlingMinimumRequirementsFailedException("could not deserialize payload", ex);
            }

            if (!SendEmailValidation.ValidationCommand(command, _logger))
                throw new HandlingMinimumRequirementsFailedException("validation information is not valid");

            Console.WriteLine(
                $"Send Email To :{command?.Email} with Subject: {command?.Subject} and Message: {command?.Message}");

            eventAckNack.Ack(deliveryTag);
        }
    }
}