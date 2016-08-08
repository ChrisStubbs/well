namespace PH.Well.Domain.ValueObjects
{
    public class RejectEvent
    {
        public int BranchId { get; set; }

        public string InvoiceNumber { get; set; }
    }
}