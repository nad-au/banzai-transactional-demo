using System;
using System.Threading.Tasks;
using Banzai;
using ConsoleApp1.Commands.CreateBankAccountForPayer.Nodes;
using ConsoleApp1.UoW;
using ConsoleApp1.Workflow;

namespace ConsoleApp1.Commands.CreateBankAccountForPayer
{
    public class CreateBankAccountForPayerCommandHandler : ICommandHandler<CreateBankAccountForPayerCommand>
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
        
        public async Task Handle(CreateBankAccountForPayerCommand command)
        {
            var context = new CreateBankAccountForPayerContext(command);
            
            var workflow = _workflowBuilder
                .AddNode<GetPayerDetails>()
                .AddNode<CreateBankAccountModel>()
                .AddNode<ValidateBankAccount>()
                .AddNode<ResetPrimaryForOtherBankAccounts>()
                .AddNode<StoreBankAccount>()
                .Build();
                
            var result = await workflow.ExecuteAsTransactional(_uow, context);

            if (result.Status == NodeResultStatus.Succeeded)
            {
                await Console.Out.WriteLineAsync("Success");
            }
        }
    }
}