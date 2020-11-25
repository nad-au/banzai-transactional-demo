using System.Threading.Tasks;

namespace ConsoleApp1.Commands
{
    public interface ICommandHandler<T>
    {
        Task Handle(T command);
    }
}