namespace PH.Well.Domain.ValueObjects
{
    public class AppSearchResult
    {
        public int? StopId { get; set; }
        public int? RouteId { get; set; }
        public int? BranchId { get; set; }
        public string InvoiceNumber { get; set; }
    }
}