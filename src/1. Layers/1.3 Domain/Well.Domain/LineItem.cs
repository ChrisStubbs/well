namespace PH.Well.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Enums;

    public class LineItem : Entity<int>
    {
        public LineItem()
        {
            this.LineItemActions = new List<LineItemAction>();
        }

        public int LineNumber { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public decimal NetPrice { get; set; }
        public int? AmendedDeliveryQuantity { get; set; }
        public int? AmendedShortQuantity { get; set; }
        public int? OriginalShortQuantity { get; set; }
        public int ActivityId { get; set; }
        public int? OriginalDespatchQuantity { get; set; }
        public int? DeliveredQuantity { get; set; }
        public string DriverReason { get; set; }
        public int JobId { get; set; }

        public decimal TotalCreditValue => LineItemActions.Where(x => x.DeliveryAction == DeliveryAction.Credit).Sum(x => x.Quantity) * NetPrice;
        public decimal TotalActionValue => LineItemActions.Sum(x => x.Quantity) * NetPrice;
        public int TotalCreditQty => LineItemActions.Where(x => x.DeliveryAction == DeliveryAction.Credit).Sum(x => x.Quantity);
        public int TotalQty => LineItemActions.Sum(x => x.Quantity);

        public int TotalShortQty => LineItemActions.Where(x => x.ExceptionType == ExceptionType.Short).Sum(x => x.Quantity);
        public int TotalDamageQty => LineItemActions.Where(x => x.ExceptionType == ExceptionType.Damage).Sum(x => x.Quantity);
        public int TotalBypassQty => LineItemActions.Where(x => x.ExceptionType == ExceptionType.Bypass).Sum(x => x.Quantity);

        public List<LineItemAction> LineItemActions { get; set; }

    }
}
