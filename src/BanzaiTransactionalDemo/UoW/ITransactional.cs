using System.Threading.Tasks;

namespace BanzaiTransactionalDemo.UoW
{
    public interface ITransactional
    {
        Task StartAsync();

        Task CommitAsync(bool autoRollback);

        Task RollbackAsync();
    }
}