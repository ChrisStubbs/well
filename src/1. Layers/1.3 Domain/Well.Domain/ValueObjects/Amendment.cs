namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;

    public class Amendment
    {
        public Amendment()
        {
            this.AmendmentLines = new List<AmendmentLine>();
        }

        public int JobId { get; set; }
        public string InvoiceNumber { get; set; }
        public string AccountNumber { get; set; }
        public int BranchId { get; set; }
        public string AmenderName { get; set; }
        public string CustomerReference { get; set; }

        public List<AmendmentLine> AmendmentLines { get; set; }
    }

    public class AmendmentLine
    {
        public int JobId { get; set; }
        public string ProductCode { get; set; }
        public int DeliveredQuantity { get; set; }
        public int ShortTotal { get; set; }
        public int DamageTotal { get; set; }
        public int RejectedTotal { get; set; }
        public int AmendedDeliveredQuantity { get; set; }
        public int AmendedShortTotal { get; set; }
        public int AmendedDamageTotal { get; set; }
        public int AmendedRejectedTotal { get; set; }
    }

}


