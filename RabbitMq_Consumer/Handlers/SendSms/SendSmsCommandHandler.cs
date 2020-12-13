using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMq_Common.Exception;
using RabbitMq_Common.Extension;
using RabbitMq_Common.RabbitMq;
using RabbitMq_Common.RabbitMq.Event;

namespace RabbitMq_Consumer.Handlers.SendSms
{
    public class SendSmsCommandHandler : IHandleCommand
    {
        private readonly ILogger<SendSmsCommandHandler> _logger;

        public SendSmsCommandHandler(ILogger<SendSmsCommandHandler> logger)
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

            SendSmsCommand command;
            try
            {
                command = message.Payload.ObjectDeserializer<SendSmsCommand>();
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
                $"Send Sms To :{command?.Mobile} with Message: {command?.Message}");

            eventAckNack.Ack(deliveryTag);
        }

        private bool ValidationCommand(SendSmsCommand command)
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(command?.Mobile))
            {
                _logger?.LogError($"invalid Mobile Number: empty or null, can not process nack/no-retry");

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(command?.Message))
            {
                _logger?.LogError($"invalid Sms Message: empty or null, can not process nack/no-retry");

                isValid = false;
            }

            //TODO: Check Regex Mobile

            return isValid;
        }
    }
}