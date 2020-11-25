using System;
using System.Collections.Generic;
using Dawn;

namespace BanzaiTransactionalDemo.Entities
{
    public partial class BankAccount
    {
        private BankAccount() {
            Bsb = "";
            AccountName = "";
            AccountNumber = "";
            ReferenceNumber = "";
            BankIdentityNumber = "";
        }

        public BankAccount(string bsb, string accountNumber, string accountName, string referenceNumber) : this() {
            Bsb = Guard.Argument(bsb, nameof(bsb)).NotNull();
            AccountName = Guard.Argument(accountName, nameof(accountName)).NotNull();
            AccountNumber = Guard.Argument(accountNumber, nameof(accountNumber)).NotNull();
            ReferenceNumber = Guard.Argument(referenceNumber, nameof(referenceNumber)).NotNull();
        }

        public string Bsb { get; set; }
        public string ReferenceNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public bool IsPrimary { get; set; }

        public string BankIdentityNumber { get; set; }

        public bool IsActive { get; set; }

        public long Id { get; set; }
        public DateTime? DateCreatedUtc { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
    }
}