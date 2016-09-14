namespace PH.Well.Domain
{
    using System;
    using Enums;

    public class Audit : Entity<int>
    {
        public string Entry { get; set; }
        public AuditType Type { get; set; }
        public string InvoiceNumber { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
