namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;

    public class AppSearchResult
    {
        public int? BranchId { get; set; }

        public IEnumerable<IAppSearchItem> Items { get; set; }
    }
}