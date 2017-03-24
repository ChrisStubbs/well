namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;
    using Enums;

    public class Delivery
    {
        public Delivery()
        {
            Lines = new List<DeliveryLine>();
        }

        public int Id { get; set; } 

        public IList<DeliveryLine> Lines { get; set; }

        public string RouteNumber { get; set; }

        public DateTime RouteDate { get; set; }

        public int DropId { get; set; }

        public string InvoiceNumber { get; set; }

        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public string JobStatus { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string FormattedDeliveryDate => this.DeliveryDate.ToShortDateString();

        public string Reason { get; set; }

        public string Action { get; set; }

        public string Assigned { get; set; }

        public string AccountId { get; set; }

        public int BranchId { get; set; }

        public string IdentityName { get; set; }
        public string CurrentUserIdentity { get; set; }

        public string CashOnDelivery { get; set; }
        public bool IsCashOnDelivery => string.IsNullOrWhiteSpace(CashOnDelivery) == false;

        public bool CanAction => string.Equals(CurrentUserIdentity, IdentityName, StringComparison.OrdinalIgnoreCase) &&
                                 JobStatus != Enums.JobStatus.Resolved.ToString();

        public bool CanSubmit => CanAction && Lines.All(l => l.CanSubmit);

        public string TotalCredit { get; set; }

        public string PendingCreditCreatedBy { get; set; }

        public string FormattedPendingCreditCreatedBy {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.PendingCreditCreatedBy) && this.PendingCreditCreatedBy.Contains("\\"))
                {
                    return this.PendingCreditCreatedBy.Split('\\')[1];
                }

                return this.PendingCreditCreatedBy;
            }
        }

        public decimal TotalCreditValueForThreshold => Lines.Sum(d => d.CreditValueForThreshold());

        public ThresholdLevel? ThresholdLevel { get; set; }

        public decimal ThresholdAmount { get; set; }

        public string CreditThresholdLevel
            => ThresholdLevel != null ? Enum<ThresholdLevel>.GetDescription(ThresholdLevel) : "";

        public bool ThresholdLevelValid => this.ThresholdAmount >= this.TotalCreditValueForThreshold;

        public int TotalOutersShort { get; set; }

        public bool IsPendingCredit { get; set; }

        public ProofOfDelivery? ProofOfDelivery { get; set; }
        public bool IsProofOfDelivery => ProofOfDelivery.HasValue;

        public bool IsAssignedTo(string identityName)
        {
            return string.Equals(IdentityName, identityName, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsOutstanding => DeliveryDate.Date < DateTime.Today.Date;

        public bool OuterDiscrepancyFound { get; set; }

        public int ToBeAdvisedCount => OuterDiscrepancyFound ? TotalOutersShort : 0;

        public bool CanBulkCredit => Lines.All(l => l.HasNoActions);
    }
}
