namespace PH.Well.Api.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Enums;

    public class DeliveryDetailModel
    {
        public DeliveryDetailModel()
        {
            this.ExceptionDeliveryLines = new List<DeliveryLineModel>();
            this.CleanDeliveryLines = new List<DeliveryLineModel>();
        }

        public int Id { get; set; }

        public string AccountCode { get; set; }

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

        public int BranchId { get; set; }

        public int GrnProcessType { get; set; }

        public string CashOnDelivery { get; set; }

        public ProofOfDelivery? ProofOfDelivery { get; set; }

        public bool IsProofOfDelivery { get; set; }

        public bool CanAction { get; set; }

        public bool CanSubmit { get {return CanAction && ExceptionDeliveryLines.Any(dl => dl.Actions.Any(a => a.Status == ActionStatus.Draft)); } }

        public bool IsPendingCredit { get; set; }

        public List<DeliveryLineModel> ExceptionDeliveryLines { get; set; }

        public List<DeliveryLineModel> CleanDeliveryLines { get; set; }
    }
}