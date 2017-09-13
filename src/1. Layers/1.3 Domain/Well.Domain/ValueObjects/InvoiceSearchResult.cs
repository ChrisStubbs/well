namespace PH.Well.Domain.ValueObjects
{
    public class InvoiceSearchResult
    {
        public int BranchId { get; set; }
        public int InvoiceId { get; set; }
        //public string InvoiceNumber { get; set; }

        public InvoiceSearchResult(){}

        public InvoiceSearchResult(int branchId, int invoiceId)
        {
            BranchId = branchId;
            InvoiceId = invoiceId;
        }

        //public InvoiceSearchResult(int branchId, string invoiceNumber)
        //{
        //    BranchId = branchId;
        //    InvoiceNumber = invoiceNumber;
        //}
    }
}