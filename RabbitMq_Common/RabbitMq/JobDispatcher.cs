using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMq_Common.Exception;
using RabbitMq_Common.Extension;
using RabbitMq_Common.RabbitMq.Event;

namespace RabbitMq_Common.RabbitMq
{
    public class JobDispatcher : IJobDispatcher
    {
        private readonly ILogger<JobDispatcher> _logger;

        private readonly Dictionary<string, IHandleCommand> _handlers = new Dictionary<string, IHandleCommand>();

        public JobDispatcher(ILogger<JobDispatcher> logger)
        {
            _logger = logger;
        }

        public async Task DispatchAsync(IEvent message, string exchange, IEventAckNack ack)
        {
            ulong deliveryTag = message.GetContext<ulong>("deliveryTag");


            if (string.IsNullOrWhiteSpace(message.Payload))
            {
                _logger?.LogError(
                    $"empty payload received from {message.RoutingKey}");

                ack.Nack(deliveryTag, false);

                return;
            }

            var handler = _handlers
                .SingleOrDefault(hc => hc.Key.Equals(exchange))
                .Value;

            if (handler != null && handler.CanHandle(message))
            {
                try
                {
                    await handler.HandleAsync(message, ack);

                    return;
                }
                catch (HandlingMinimumRequirementsFailedException ex)
                {
                    _logger?.LogError(ex, ex.Message);

                    ack.Nack(deliveryTag, false);

                    return;
                }
                catch (System.Exception ex)
                {
                    _logger?.LogError(ex, ex.Message);

                    throw;
                }
            }

            throw new CouldNotFindAnyHandlerException(
                $"non of handlers could not handle this type of event: {message.Payload}");
        }

        public void RegisterHandler(string exchange, IHandleCommand handler)
        {
            _handlers.Add(exchange, handler);
        }
    }
}