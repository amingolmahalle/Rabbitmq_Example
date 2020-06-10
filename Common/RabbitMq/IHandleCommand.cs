using System.Threading.Tasks;

namespace Common.RabbitMq
{
    public interface IHandleCommand<in T> : IHandleCommand where T : ICommand
    {
        Task Handle(T message);
    }

    public interface IHandleCommand
    {
    }
}