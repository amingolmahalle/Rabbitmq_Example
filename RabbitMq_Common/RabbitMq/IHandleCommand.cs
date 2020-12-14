using System.Threading.Tasks;
using RabbitMq_Common.RabbitMq.Event;

namespace RabbitMq_Common.RabbitMq
{
    public interface IHandleCommand
    {
        bool CanHandle(IEvent message);

        Task HandleAsync(IEvent message, ulong deliveryTag, IEventAckNack eventAckNack);
    }
}