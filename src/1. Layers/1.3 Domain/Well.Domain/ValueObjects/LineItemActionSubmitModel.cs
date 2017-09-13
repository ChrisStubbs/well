namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LineItemActionSubmitModel : LineItemAction
    {
        public int JobId { get; set; }
        public string Stop { get; set; }
        public string InvoiceNumber { get; set; }
        public int BranchId { get; set; }
        public string ProductCode { get; set; }
        public decimal NetPrice { get; set; }
        public DateTime RouteDate { get; set; }
        public decimal TotalValue => Quantity * NetPrice;

        public static IEnumerable<LineItemActionSubmitModel> GetItemsContainingJobIds(IList<LineItemActionSubmitModel> items, IList<int> jobIds)
        {
            return items.Where(x => jobIds.Contains(x.JobId));
        }
    }
}