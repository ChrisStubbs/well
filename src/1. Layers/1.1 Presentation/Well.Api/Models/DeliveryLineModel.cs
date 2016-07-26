namespace PH.Well.Api.Models
{
    public class DeliveryLineModel
    {
        public int LineNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string Value { get; set; }
        public int InvoicedQuantity { get; set; }
        public int DeliveredQuantity { get; set; }
        public int DamagedQuantity { get; set; }
        public int ShortQuantity { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public bool IsException => Status != "Complete";
    }
}
