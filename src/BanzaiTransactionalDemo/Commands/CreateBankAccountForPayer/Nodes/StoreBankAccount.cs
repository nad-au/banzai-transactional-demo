using System;
using System.Threading.Tasks;
using Banzai;
using BanzaiTransactionalDemo.UoW;
using BanzaiTransactionalDemo.Workflow;
using Dawn;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes
{
    public class StoreBankAccount : TransactionalNode<CreateBankAccountForPayerContext>
    {
        private readonly BankAccountRepository _bankAccountRepository;
        
        public StoreBankAccount(BankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }
        
        // Return transactional dependencies so workflow executor can subscribe them to UoW Builder
        public override ITransactional[] GetTransactionals()
        {
            return new ITransactional[] {_bankAccountRepository};
        }

        // Context / state pre-checks to ensure successful node execution
        protected override void OnBeforeExecute(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            Guard.Argument(context.Subject.NewBankAccount).NotNull();
        }

        protected override async Task<NodeResultStatus> PerformExecuteAsync(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            await _bankAccountRepository.Add(context.Subject.NewBankAccount);
            
            await Console.Out.WriteLineAsync($"Executed {nameof(StoreBankAccount)}");

            return NodeResultStatus.Succeeded;
        }
    }
}