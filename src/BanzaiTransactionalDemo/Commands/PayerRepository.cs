using System.Threading.Tasks;
using BanzaiTransactionalDemo.Entities;
using BanzaiTransactionalDemo.UoW;

namespace BanzaiTransactionalDemo.Commands
{
    public class PayerRepository : ITransactional
    {
        public Task<Payer> Get(long id) => Task.FromResult(new Payer(string.Empty));
        
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