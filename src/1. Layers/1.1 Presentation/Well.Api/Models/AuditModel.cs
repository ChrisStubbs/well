namespace PH.Well.Api.Models
{
    using System;

    public class AuditModel
    {
        public string Entry { get; set; }
        public string Type { get; set; }
        public string InvoiceNumber { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string Account => string.IsNullOrWhiteSpace(AccountName) ? AccountCode : string.Join(" - ", AccountCode, AccountName);

        public string DeliveryDate { get; set; }
        public DateTime AuditDate { get; set; }
        public string AuditBy { get; set; }
    }
}