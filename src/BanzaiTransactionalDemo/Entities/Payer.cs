using System;
using Dawn;

namespace BanzaiTransactionalDemo.Entities
{
    public partial class Payer
    {
        private Payer() {
            ExternalReferenceId = null!;
        }

        public Payer(string externalReferenceId) {
            ExternalReferenceId = Guard.Argument(externalReferenceId, nameof(externalReferenceId)).NotNull();
        }

        public string ExternalReferenceId { get; set; }
        public bool IsSetupComplete { get; set; }
        public DateTime? SetupCompletedOnUtc { get; set; }

        public bool IsActive { get; set; }

        public long Id { get; set; }
        public DateTime? DateCreatedUtc { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
    }
}
