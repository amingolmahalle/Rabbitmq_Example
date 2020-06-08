using System.Threading.Tasks;

namespace Common.RabbitMq
{
    public interface IHandleCommand<in T> where T : ICommand
    {
        Task Handle(T message);
    }
}