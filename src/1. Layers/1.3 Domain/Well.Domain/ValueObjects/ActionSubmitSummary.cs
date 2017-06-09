namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;

    public class ActionSubmitSummary
    {
        public ActionSubmitSummary()
        {
            Items = new List<ActionSubmitSummaryItem>();
            JobIds = new List<int>();
        }

        public List<int> JobIds { get; set; }
        public string Summary { get; set; }
        public List<ActionSubmitSummaryItem> Items { get; set; }
    }
}