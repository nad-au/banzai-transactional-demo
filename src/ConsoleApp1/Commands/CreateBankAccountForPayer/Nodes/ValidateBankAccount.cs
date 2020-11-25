using System;
using System.Threading.Tasks;
using Banzai;
using ConsoleApp1.UoW;
using ConsoleApp1.Workflow;

namespace ConsoleApp1.Commands.CreateBankAccountForPayer.Nodes
{
    public class ValidateBankAccount : TransactionalNode<CreateBankAccountForPayerContext>
    {
        private readonly Transactional3 _transactional3;
        
        public ValidateBankAccount(Transactional3 transactional3)
        {
            _transactional3 = transactional3;
        }
        
        protected override async Task<NodeResultStatus> PerformExecuteAsync(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            await Console.Out.WriteLineAsync($"Executed {nameof(ValidateBankAccount)}");

            return NodeResultStatus.Succeeded;
        }

        public override ITransactional[] GetTransactionals()
        {
            return new ITransactional[] {_transactional3};
        }
    }
}