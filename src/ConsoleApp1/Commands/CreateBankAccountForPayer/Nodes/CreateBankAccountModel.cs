using System;
using System.Threading.Tasks;
using Banzai;

namespace ConsoleApp1.Commands.CreateBankAccountForPayer.Nodes
{
    public class CreateBankAccountModel : Node<CreateBankAccountForPayerContext>
    {
        protected override async Task<NodeResultStatus> PerformExecuteAsync(IExecutionContext<CreateBankAccountForPayerContext> context)
        {
            await Console.Out.WriteLineAsync($"Executed {nameof(CreateBankAccountModel)}");

            var cmdContext = context.Subject;
            await Console.Out.WriteLineAsync($"{nameof(cmdContext.ExtraProp1)}: {cmdContext.ExtraProp1}");

            cmdContext.ExtraProp1 = "Bar";

            return NodeResultStatus.Succeeded;
        }
    }
}