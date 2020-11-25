using BanzaiTransactionalDemo.Entities;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Validators
{
    public class BankAccountValidator
    {
        public bool Validate(BankAccount bankAccount) => true;
    }
}