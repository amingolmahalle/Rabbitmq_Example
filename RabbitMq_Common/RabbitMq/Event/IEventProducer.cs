using System;
using System.Threading.Tasks;

namespace RabbitMq_Common.RabbitMq.Event
{
    public interface IRabbitConnector : IDisposable
    {
        Task ConsumeAsync(string queueName);

        Task<bool> RaiseEventAsync(IEvent e);

        Task InitializeAsync(string serviceName = "");
    }
}