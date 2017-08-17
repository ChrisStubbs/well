namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;

    public class AppSearchResultSummary
    {
        //public int[] StopIds { get; set; }
        public int[] RouteIds { get; set; }
        public int[] LocationIds { get; set; }
        public int[] InvoiceIds { get; set; }

        public static AppSearchResultSummary Get(IEnumerable<AppSearchResult> searchResults)
        {
            var summary = new AppSearchResultSummary
            {
                //StopIds = searchResults.Where(x => x.StopId.HasValue).Select(x => x.StopId.Value).Distinct().ToArray(),
                RouteIds = searchResults.Where(x => x.RouteId.HasValue).Select(x => x.RouteId.Value).Distinct()
                    .ToArray(),
                LocationIds = searchResults.Where(x => x.BranchId.HasValue && x.LocationId.HasValue)
                    .Select(x => x.LocationId.Value).Distinct().ToArray(),
                InvoiceIds = searchResults.Where(x => x.BranchId.HasValue && x.InvoiceId.HasValue)
                    .Select(x => x.InvoiceId.Value).Distinct().ToArray()
                
                //Invoices = searchResults.Where(x => x.BranchId.HasValue && !string.IsNullOrWhiteSpace(x.InvoiceNumber))
                //    .Select(x => new InvoiceSearchResult(x.BranchId.Value, x.InvoiceNumber)).ToArray()
            };
            return summary;
        }
    }
}