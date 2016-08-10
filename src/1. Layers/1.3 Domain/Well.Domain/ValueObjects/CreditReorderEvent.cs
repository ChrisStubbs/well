namespace PH.Well.Domain.ValueObjects
{
    public class CreditReorderEvent
    {
        public int BranchId { get; set; }

        public string InvoiceNumber { get; set; }
    }
}