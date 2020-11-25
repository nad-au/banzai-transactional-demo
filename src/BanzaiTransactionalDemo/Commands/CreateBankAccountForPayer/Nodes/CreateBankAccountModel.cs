using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Banzai;
using BanzaiTransactionalDemo.Entities;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes
{
    public class CreateBankAccountModel : Node<CreateBankAccountForPayerContext>
    {
        protected override async Task<NodeResultStatus> PerformExecuteAsync(
            IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            var command = context.Subject.Data;

            context.Subject.NewBankAccount = new BankAccount(
                command.Bsb,
                command.AccountNumber,
                command.AccountName,
                Guid.NewGuid().ToString())
            {
                IsPrimary = command.IsPrimary,
            };

            await Console.Out.WriteLineAsync($"Executed {nameof(CreateBankAccountModel)}");

            return NodeResultStatus.Succeeded;
        }
    }
}