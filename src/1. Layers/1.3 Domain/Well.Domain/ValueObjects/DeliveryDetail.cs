namespace PH.Well.Domain.ValueObjects
{
    using Enums;

    public class DeliveryDetail
    {
        public int Id { get; set; } 
    
        public string AccountCode { get; set; }
        public PerformanceStatus Status { get; set; }
        public string AccountName { get; set; }
        public string AccountAddress { get; set; }
        public string InvoiceNumber { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string DeliveryType { get; set; }

        public bool IsException => ExceptionStatuses.Statuses.Contains(Status);
    }
}
