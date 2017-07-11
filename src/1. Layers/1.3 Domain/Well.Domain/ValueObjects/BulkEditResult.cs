namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Enums;

    public class BulkEditResult
    {
        public BulkEditResult()
        {
            Statuses = new List<JobIdResolutionStatus>();
            LineItemIds = new List<int>();
        }

        public IList<JobIdResolutionStatus> Statuses { get; set; }
        public IList<int> LineItemIds { get; set; }
    }

}