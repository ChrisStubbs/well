namespace PH.Well.Api.Models
{
    public class ResolvedDeliveryModel
    {
        public string Route { get; set; }
        public string Drop { get; set; }
        public int InvoiceNo { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string Assigned { get; set; }
        public string DateTime { get; set; }      
    }
}