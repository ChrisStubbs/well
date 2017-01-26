namespace PH.Well.Domain.ValueObjects
{
    using System;

    public class Delivery
    {
        public int Id { get; set; } 

        public string RouteNumber { get; set; }

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

        public string CashOnDelivery { get; set; }

        public bool CanAction { get; private set; }

        public string TotalCredit { get; set; }

        public string PendingCreditCreatedBy { get; set; }

        public string Cod { get; set; }

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

        public decimal TotalCreditValueForThreshold { get; set; }

        public void SetCanAction(string username)
        {
            this.CanAction = username.Equals(this.IdentityName, StringComparison.OrdinalIgnoreCase);
        }

        public int TotalOutersShort { get; set; }

    }
}
