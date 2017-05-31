namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using Enums;

    public class BulkActionResults
    {
        public BulkActionResults()
        {
            Items = new List<BulkActionResult>();
        }
        public List<BulkActionResult> Items { get; set; }
        public string ResultSummary { get; set; }

        public bool IsActionValid { get; set; }

        //public bool HasWarnings => Results.Any(x => x.Type == BulkActionResultType.Warning);
        //public bool HasErrors => Results.Any(x => x.Type == BulkActionResultType.Error);
    }
}