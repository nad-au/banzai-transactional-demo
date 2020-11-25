using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Banzai;
using BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes;
using BanzaiTransactionalDemo.UoW;
using BanzaiTransactionalDemo.Workflow;
using MediatR;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer
{
    public class CreateBankAccountForPayerCommandHandler : IRequestHandler<CreateBankAccountForPayerCommand, CommandResponse>
    {
        private readonly IWorkflowBuilder<CreateBankAccountForPayerContext> _workflowBuilder;
        private readonly IUnitOfWorkBuilder _uow;

        public CreateBankAccountForPayerCommandHandler(
            IWorkflowBuilder<CreateBankAccountForPayerContext> workflowBuilder,
            IUnitOfWorkBuilder uow)
        {
            _workflowBuilder = workflowBuilder;
            _uow = uow;
        }
        
        public async Task<CommandResponse> Handle(CreateBankAccountForPayerCommand request, CancellationToken cancellationToken)
        {
            var context = new CreateBankAccountForPayerContext(request);
            
            var workflow = _workflowBuilder
                .AddNode<GetPayerDetails>()
                .AddNode<CreateBankAccountModel>()
                .AddNode<ValidateBankAccount>()
                .AddNode<ResetPrimaryForOtherBankAccounts>()
                .AddNode<StoreBankAccount>()
                .Build();
                
            var result = await workflow.ExecuteAsTransactional(_uow, context);

            return new CommandResponse
            {
                Success = result.Status == NodeResultStatus.Succeeded,
                Messages = result.GetErrorMessages().ToList()
            };
        }
    }
}