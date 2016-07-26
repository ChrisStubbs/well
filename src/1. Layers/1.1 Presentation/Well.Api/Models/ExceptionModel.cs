namespace PH.Well.Api.Models
{
    public class ExceptionModel
    {
        public string Route { get; set; }
        public string Drop { get; set; }
        public string InvoiceNo { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Assigned { get; set; }
        public string DateTime { get; set; }
    }
}