using System.Threading.Tasks;

namespace BanzaiTransactionalDemo.Commands
{
    public interface ICommandHandler<T>
    {
        Task Handle(T command);
    }
}