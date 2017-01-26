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

        public string DeliveryType { get; set; }

        public bool CanAction { get; set; }

        public string GrnNumber { get; set; }

        public string IdentityName { get; set; }

        public bool IsException => ExceptionStatuses.Statuses.Contains(Status);

        public void SetCanAction(string username)
        {
            this.CanAction = username.Equals(this.IdentityName, StringComparison.OrdinalIgnoreCase);
        }

        public int BranchId { get; set; }

        public int GrnProcessType { get; set; }
    }
}
