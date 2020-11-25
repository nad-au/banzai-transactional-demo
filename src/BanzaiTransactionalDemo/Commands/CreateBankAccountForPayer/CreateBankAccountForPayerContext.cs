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
        
        public CreateBankAccountForPayerCommand Data { get; private set; }

        public BankAccount NewBankAccount { get; set; }
        
        public string ExtraProp1 { get; set; }
    }
}