namespace PH.Well.Domain.ValueObjects
{
    public class DeliveryLine
    {
        public int Id { get; set; }
        public int LineNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public int Value { get; set; }
        public int InvoicedQuantity { get; set; }
        public int DeliveredQuantity { get; set; }
        public int DamagedQuantity { get; set; }
        public int ShortQuantity { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
