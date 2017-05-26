namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;

    public class AppSearchResultSummary
    {
        public int[] StopIds { get; set; }
        public  int[] RouteIds { get; set; }
    
        public static AppSearchResultSummary Get(IEnumerable<AppSearchResult> searchResults)
        {
            var summary = new AppSearchResultSummary
            {
                StopIds = searchResults.Where(x => x.StopId.HasValue).Select(x => x.StopId.Value).Distinct().ToArray(),
                RouteIds = searchResults.Where(x => x.RouteId.HasValue).Select(x => x.RouteId.Value).Distinct().ToArray()
            };
            return summary;
        }
    }
}