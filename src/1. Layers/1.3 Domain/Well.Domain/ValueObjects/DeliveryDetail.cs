namespace PH.Well.Domain.ValueObjects
{
    using System;

    using Enums;

    public class DeliveryDetail
    {
        public int Id { get; set; } 
    
        public string AccountCode { get; set; }

        public PerformanceStatus Status { get; set; }

        public int OuterCount { get; set; }

        public bool OuterDiscrepancyFound { get; set; }

        public int TotalOutersShort { get; set; }

        public string AccountName { get; set; }

        public string AccountAddress { get; set; }

        public string InvoiceNumber { get; set; }

        public string ContactName { get; set; }

        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }

        public string JobStatus { get; set; }

        public string GrnNumber { get; set; }

        public string IdentityName { get; set; }

        public string CurrentUserIdentity { get; set; }

        public bool CanAction => string.Equals(CurrentUserIdentity, IdentityName, StringComparison.OrdinalIgnoreCase) &&
                                 JobStatus != Enums.JobStatus.Resolved.ToString();

        public bool IsPendingCredit { get; set; }

        public int BranchId { get; set; }

        public int GrnProcessType { get; set; }

        public string CashOnDelivery { get; set; }

        public ProofOfDelivery? ProofOfDelivery { get; set; }

        public bool IsProofOfDelivery => ProofOfDelivery.HasValue;
    }
}
