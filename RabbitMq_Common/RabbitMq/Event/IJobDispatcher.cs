using System.Threading.Tasks;

namespace RabbitMq_Common.RabbitMq.Event
{
    public interface IJobDispatcher
    {
        Task DispatchAsync(IEvent ev, string exchange, IEventAckNack eventAckNack);

        void RegisterHandler(string exchange, IHandleCommand handler);
    }
}