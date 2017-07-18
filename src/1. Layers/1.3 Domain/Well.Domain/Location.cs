namespace PH.Well.Domain
{
    public class Location
    {
        public int BranchId { get; set; }
        public string Branch { get; set; }
        public string PrimaryAccountNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public int TotalInvoices { get; set; }
        public int Exceptions { get; set; }
    }
}
