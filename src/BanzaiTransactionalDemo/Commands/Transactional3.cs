using System.Threading.Tasks;
using BanzaiTransactionalDemo.UoW;

namespace BanzaiTransactionalDemo.Commands
{
    public class Transactional3 : ITransactional
    {
        public Task StartAsync()
        {
            return Task.CompletedTask;
        }

        public Task CommitAsync(bool autoRollback)
        {
            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            return Task.CompletedTask;
        }
    }
}