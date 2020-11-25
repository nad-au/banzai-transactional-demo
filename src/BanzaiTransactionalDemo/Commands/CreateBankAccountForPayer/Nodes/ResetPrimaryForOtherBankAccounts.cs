using System;
using System.Threading.Tasks;
using Banzai;
using BanzaiTransactionalDemo.UoW;
using BanzaiTransactionalDemo.Workflow;
using Dawn;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes
{
    public class ResetPrimaryForOtherBankAccounts : TransactionalNode<CreateBankAccountForPayerContext>
    {
        private readonly BankAccountRepository _bankAccountRepository;
        
        public ResetPrimaryForOtherBankAccounts(BankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        // Return transactional dependencies so workflow executor can subscribe them to UoW Builder
        public override ITransactional[] GetTransactionals()
        {
            return new ITransactional[] {_bankAccountRepository};
        }

        // Conditionally execute
        public override Task<bool> ShouldExecuteAsync(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            return Task.FromResult(context.Subject.Data.IsPrimary);
        }

        protected override void OnBeforeExecute(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            var command = context.Subject.Data;
            
            Guard.Argument(command.Bsb, nameof(command.Bsb)).NotNull();
            Guard.Argument(command.AccountNumber, nameof(command.AccountNumber)).NotNull();
            Guard.Argument(command.AccountName, nameof(command.AccountName)).NotNull();
        }

        protected override async Task<NodeResultStatus> PerformExecuteAsync(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            await _bankAccountRepository.ResetAllPrimaryAccounts();
            
            await Console.Out.WriteLineAsync($"Executed {nameof(ResetPrimaryForOtherBankAccounts)}");

            return NodeResultStatus.Succeeded;
        }
    }
}