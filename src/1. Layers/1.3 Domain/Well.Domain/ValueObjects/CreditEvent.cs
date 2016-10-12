namespace PH.Well.Domain.ValueObjects
{
    public class CreditEvent
    {
        /// <summary>
        /// Job Id
        /// </summary>
        public int Id { get; set; }

        public int BranchId { get; set; }

        public string InvoiceNumber { get; set; }

        public int TotalCreditValueForThreshold { get; set; }
    }
}