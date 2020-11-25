using System;
using System.Threading.Tasks;
using Banzai;
using BanzaiTransactionalDemo.UoW;
using BanzaiTransactionalDemo.Workflow;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes
{
    public class StoreBankAccount : TransactionalNode<CreateBankAccountForPayerContext>
    {
        private readonly Transactional3 _transactional3;
        
        public StoreBankAccount(Transactional3 transactional3)
        {
            _transactional3 = transactional3;
        }
        
        protected override async Task<NodeResultStatus> PerformExecuteAsync(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            await Console.Out.WriteLineAsync($"Executed {nameof(StoreBankAccount)}");

            var cmdContext = context.Subject;
            await Console.Out.WriteLineAsync($"{nameof(cmdContext.ExtraProp1)}: {cmdContext.ExtraProp1}");

            return NodeResultStatus.Succeeded;
        }

        public override ITransactional[] GetTransactionals()
        {
            return new ITransactional[] {_transactional3};
        }
    }
}