namespace PH.Well.Domain
{
    using System.Collections.Generic;

    public class LineItem : Entity<int>
    {
        public LineItem()
        {
            this.LineItemActions = new List<LineItemAction>();
        }

        public int LineNumber { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public int? AmendedDeliveryQuantity { get; set; }
        public int? AmendedShortQuantity { get; set; }
        public int? OriginalShortQuantity { get; set; }
        public int ActivityId { get; set; }

        public List<LineItemAction> LineItemActions { get; set; }

    }
}
