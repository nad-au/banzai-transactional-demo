using System.Threading.Tasks;
using ConsoleApp1.UoW;

namespace ConsoleApp1.Commands
{
    public class Transactional1 : ITransactional
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