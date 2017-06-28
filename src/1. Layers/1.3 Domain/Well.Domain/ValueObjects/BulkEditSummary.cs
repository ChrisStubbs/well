namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;

    public class BulkEditSummary
    {
        public BulkEditSummary()
        {
            Items = new List<BulkEditItem>();
        }

        public string Message => $"You will be editing {Items.Count} Jobs with a total value of £{Items.Sum(x => x.TotalValue)}";
        public IList<BulkEditItem> Items { get; set; }
    }
}