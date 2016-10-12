namespace PH.Well.Domain.ValueObjects
{
    public class PendingCreditDetail
    {
        public int LineNumber { get; set; }

        public int ProductCode { get; set; }

        public string ProductDescription { get; set; }

        public string Value { get; set; }

        public int InvoiceQuantity { get; set; }

        public int DeliveredQuantity { get; set; }

        public int DamagedQuantity { get; set; }

        public int ShortQuantity { get; set; }

        public int ActionQuantity { get; set; }

        public string Action { get; set; }
    }
}