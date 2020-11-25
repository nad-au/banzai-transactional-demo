using System.Threading.Tasks;

namespace ConsoleApp1.UoW
{
    public interface ITransactional
    {
        Task StartAsync();

        Task CommitAsync(bool autoRollback);

        Task RollbackAsync();
    }
}