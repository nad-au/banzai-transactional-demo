using System.Threading.Tasks;
using BanzaiTransactionalDemo.Entities;
using BanzaiTransactionalDemo.UoW;

namespace BanzaiTransactionalDemo.Commands
{
    public class BankAccountRepository : ITransactional
    {
        public Task ResetAllPrimaryAccounts() => Task.CompletedTask;
        public Task Add(BankAccount bankAccount) => Task.CompletedTask;
        
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