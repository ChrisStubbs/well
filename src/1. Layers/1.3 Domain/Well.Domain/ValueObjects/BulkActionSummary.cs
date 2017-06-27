//namespace PH.Well.Domain.ValueObjects
//{
//    using System.Collections.Generic;
//    using System.Linq;

//    public class BulkActionSummary
//    {
//        public BulkActionSummary()
//        {
//            WarningItems = new List<BulkActionSummaryItem>();
//            IgnoreItems = new List<BulkActionSummaryItem>();
//        }
//        public string Message { get; set; }
//        public bool HasWarnings => WarningItems.Any();
//        public bool HasItemsToIgnore => IgnoreItems.Any();
//        public IList<BulkActionSummaryItem> WarningItems { get; set; }
//        public IList<BulkActionSummaryItem> IgnoreItems { get; set; }
//    }
//}