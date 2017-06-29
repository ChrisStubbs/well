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

        public string Message
        {
            get
            {
                return Items.Any() ? $"{Items.Count} Jobs with a total value of £{Items.Sum(x => x.TotalValue)}" : $"No editable items selected";
            }
        }
            
        public IList<BulkEditItem> Items { get; set; }
    }
}