namespace PH.Well.Domain.ValueObjects
{
    public class LineItemActionSubmitModel : LineItemAction
    {
        public int JobId { get; set; }
        public string InvoiceNumber { get; set; }
        public int BranchId { get; set; }
        public string ProductCode { get; set; }
        public decimal NetPrice { get; set; }
        public decimal TotalValue => Quantity * NetPrice;
    }
}