using System;
using System.Threading.Tasks;
using Banzai;
using BanzaiTransactionalDemo.UoW;
using BanzaiTransactionalDemo.Workflow;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes
{
    public class ResetPrimaryForOtherBankAccounts : TransactionalNode<CreateBankAccountForPayerContext>
    {
        private readonly Transactional3 _transactional3;
        
        public ResetPrimaryForOtherBankAccounts(Transactional3 transactional3)
        {
            _transactional3 = transactional3;
        }
        
        protected override async Task<NodeResultStatus> PerformExecuteAsync(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            await Console.Out.WriteLineAsync($"Executed {nameof(ResetPrimaryForOtherBankAccounts)}");

            return NodeResultStatus.Succeeded;
        }

        public override ITransactional[] GetTransactionals()
        {
            return new ITransactional[] {_transactional3};
        }
    }
}