namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;

    public class PatchSummary
    {
        public PatchSummary()
        {
            Items = new List<PatchSummaryItem>();
        }

        public string Message
        {
            get
            {
                return Items.Any() 
                    ? (Items.Count==1) 
                        ?  $"One job with a total value of £{Items.Sum(x => x.TotalValue)} selected" 
                        :  $"{Items.Count} jobs with a total value of £{Items.Sum(x => x.TotalValue)} selected"
                    :"No editable exceptions selected";
            }
        }
            
        public IList<PatchSummaryItem> Items { get; set; }
    }
}