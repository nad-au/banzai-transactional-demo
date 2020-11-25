using System;
using System.Threading.Tasks;
using Banzai;
using ConsoleApp1.UoW;
using ConsoleApp1.Workflow;

namespace ConsoleApp1.Commands.CreateBankAccountForPayer.Nodes
{
    public class GetPayerDetails : TransactionalNode<CreateBankAccountForPayerContext>
    {
        private readonly Transactional1 _transactional1;
        
        public GetPayerDetails(Transactional1 transactional1)
        {
            _transactional1 = transactional1;
        }
        
        protected override async Task<NodeResultStatus> PerformExecuteAsync(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            await Console.Out.WriteLineAsync($"Executed {nameof(GetPayerDetails)}");

            var cmdContext = context.Subject;
            cmdContext.ExtraProp1 = "Foo";

            return NodeResultStatus.Succeeded;
        }

        public override ITransactional[] GetTransactionals()
        {
            return new ITransactional[] {_transactional1};
        }
    }
}