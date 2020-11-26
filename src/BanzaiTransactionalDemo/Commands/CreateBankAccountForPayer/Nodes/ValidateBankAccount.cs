using System;
using System.Threading.Tasks;
using Banzai;
using BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Validators;
using Dawn;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes
{
    public class ValidateBankAccount : Node<CreateBankAccountForPayerContext>
    {
        private readonly BankAccountValidator _bankAccountValidator;
        
        public ValidateBankAccount(BankAccountValidator bankAccountValidator)
        {
            _bankAccountValidator = bankAccountValidator;
        }
        
        // Context / state pre-checks to ensure successful node execution
        protected override void OnBeforeExecute(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            Guard.Argument(context.Subject.NewBankAccount).NotNull();
        }

        protected override async Task<NodeResultStatus> PerformExecuteAsync(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            await Console.Out.WriteLineAsync($"Executed {nameof(ValidateBankAccount)}");

            return _bankAccountValidator.Validate(context.Subject.NewBankAccount)
                ? NodeResultStatus.Succeeded
                : NodeResultStatus.Failed;
        }
    }
}