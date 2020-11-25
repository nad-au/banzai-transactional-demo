using MediatR;

namespace BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer
{
    public class CreateBankAccountForPayerCommand : IRequest<CommandResponse>
    {
        public long PayerId { get; set; }
        public string Bsb { get; set; }
        public string AccountNumber { get; set; }

        public string AccountName { get; set; }
        public bool IsPrimary { get; set; }
    }
}