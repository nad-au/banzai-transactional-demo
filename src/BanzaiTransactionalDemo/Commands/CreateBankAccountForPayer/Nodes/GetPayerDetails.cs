using System;
using System.Threading.Tasks;
using Banzai;
using BanzaiTransactionalDemo.UoW;
using BanzaiTransactionalDemo.Workflow;
using Dawn;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes
{
    public class GetPayerDetails : TransactionalNode<CreateBankAccountForPayerContext>
    {
        private readonly PayerRepository _payerRepository;
        
        public GetPayerDetails(PayerRepository payerRepository)
        {
            _payerRepository = payerRepository;
        }
        
        // Return transactional dependencies so workflow executor can subscribe them to UoW Builder
        public override ITransactional[] GetTransactionals()
        {
            return new ITransactional[] {_payerRepository};
        }

        // Context / state pre-checks to ensure successful node execution
        protected override void OnBeforeExecute(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            var command = context.Subject.Data;

            Guard.Argument(command.PayerId, nameof(command.PayerId)).GreaterThan(0);
        }

        protected override async Task<NodeResultStatus> PerformExecuteAsync(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            var payer = await _payerRepository.Get(context.Subject.Data.PayerId);
            if (payer == null)
            {
                return NodeResultStatus.Failed;
            }

            await Console.Out.WriteLineAsync($"Executed {nameof(GetPayerDetails)}");

            return NodeResultStatus.Succeeded;
        }
    }
}