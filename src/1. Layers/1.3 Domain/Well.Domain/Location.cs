using PH.Well.Domain.Enums;

namespace PH.Well.Domain
{
    public class Location
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string Branch => $"{BranchName} ({BranchId})";
        public string PrimaryAccountNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Address => $"{AddressLine1} {AddressLine2} {Postcode}";
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }
        public int TotalInvoices { get; set; }
        public int Invoiced { get; set; }
        public int Cleans => Invoiced - Exceptions;
        public int Exceptions { get; set; }
        public JobIssueType JobIssueType
        {
            get
            {
                return
                    (this.HasNotDefinedDeliveryAction ? JobIssueType.ActionRequired : JobIssueType.All) |
                    (this.NoGRNButNeeds ? JobIssueType.MissingGRN : JobIssueType.All) |
                    (this.PendingSubmission ? JobIssueType.PendingSubmission : JobIssueType.All);
            }
        }
        public bool HasNotDefinedDeliveryAction { get; set; }
        public bool NoGRNButNeeds { get; set; }
        public bool PendingSubmission { get; set; }
    }
}