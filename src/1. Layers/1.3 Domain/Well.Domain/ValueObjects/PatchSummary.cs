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
                    ? (Items.Count == 1)
                        ? $"One job with a total exception value of £{Items.Sum(x => x.TotalExceptionValue)} selected"
                        : $"{Items.Count} jobs with a total exception value of £{Items.Sum(x => x.TotalExceptionValue)} selected"
                    : "No editable exceptions selected";
            }
        }

        public string ManuallyCompleteMessage => NoOfJobs > 0
            ? (Items.Count == 1)
                ? $"One job with a total order value of £{TotalDispatchedValue} selected"
                : $"{Items.Count} jobs with a total order value of £{TotalDispatchedValue} selected"
            : "No jobs that can be manually completed selected. The Job must be assigned to you and have a status of 'Invoiced' or be marked as 'Completed on Paper'";

        public int NoOfJobs => Items.Count;
        public int TotalDispatchedQuantity => Items.Sum(x => x.TotalDispatchedQuantity);
        public decimal TotalDispatchedValue => Items.Sum(x => x.TotalDispatchedValue);

        public IList<PatchSummaryItem> Items { get; set; }
    }
}