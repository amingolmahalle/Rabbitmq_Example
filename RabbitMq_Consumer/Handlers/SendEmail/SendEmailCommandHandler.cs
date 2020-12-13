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

        public async Task HandleAsync(IEvent message, IEventAckNack eventAckNack)
        {
            ulong deliveryTag = message.GetContext<ulong>("deliveryTag");

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

            if (!ValidationCommand(command))
                throw new HandlingMinimumRequirementsFailedException("validation command is not valid");

            Console.WriteLine(
                $"Send Email To :{command?.Email} with Subject: {command?.Subject} and Message: {command?.Message}");

            eventAckNack.Ack(deliveryTag);
        }

        private bool ValidationCommand(SendEmailCommand command)
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(command?.Email))
            {
                _logger?.LogError($"invalid Email Address: empty or null, can not process nack/no-retry");

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(command?.Message))
            {
                _logger?.LogError($"invalid Email Message: empty or null, can not process nack/no-retry");

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(command?.Subject))
            {
                _logger?.LogError($"invalid Email Subject: empty or null, can not process nack/no-retry");

                isValid = false;
            }

            //TODO: Check Regex Email
            
            return isValid;
        }
    }
}