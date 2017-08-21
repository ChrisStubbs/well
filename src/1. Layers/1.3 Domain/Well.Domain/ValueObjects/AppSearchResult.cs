namespace PH.Well.Domain.ValueObjects
{
    public class AppSearchResult
    {
        // Common to all search results
        public int? BranchId { get; set; }

        // Route based search results
        public int? RouteId { get; set; }   // Found by driver or route

        //public int? StopId { get; set; }    // Unlikely to match as no searches look at stop info

        // Location based search results
        public int? LocationId { get; set; }    // Found by account number
        public int? InvoiceId { get; set; }     // Found by invoice number

        //public string InvoiceNumber { get; set; }
    }
}
