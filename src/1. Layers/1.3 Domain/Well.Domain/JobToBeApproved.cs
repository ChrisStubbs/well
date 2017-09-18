namespace PH.Well.Domain
{
    using System;

    public class JobToBeApproved
    {
        public int JobId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string Account { get; set; }
        public int AccountId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime DateSubmitted { get; set; }
        public int CreditQuantity { get; set; }
        public decimal CreditValue { get; set; }
        public string AssignedTo { get; set; }
        public int InvoiceId { get; set; }
    }
}