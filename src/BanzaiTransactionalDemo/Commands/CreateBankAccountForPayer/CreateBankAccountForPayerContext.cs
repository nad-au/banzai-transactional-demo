using BanzaiTransactionalDemo.Entities;
using BanzaiTransactionalDemo.Workflow;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer
{
    public class CreateBankAccountForPayerContext : IWorkflowContext<CreateBankAccountForPayerCommand>
    {
        public CreateBankAccountForPayerContext(CreateBankAccountForPayerCommand command)
        {
            Data = command;
        }
        
        public CreateBankAccountForPayerCommand Data { get; }

        public BankAccount NewBankAccount { get; set; }
    }
}