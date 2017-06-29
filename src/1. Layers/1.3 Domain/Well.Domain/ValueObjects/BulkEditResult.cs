namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Enums;

    public class BulkEditResult
    {
        public BulkEditResult()
        {
            Statuses = new List<BulkEditResolutionStatus>();
            LineItemIds = new List<int>();
        }

        public IList<BulkEditResolutionStatus> Statuses { get; set; }
        public IList<int> LineItemIds { get; set; }
    }

    public class BulkEditResolutionStatus
    {
        public BulkEditResolutionStatus(int jobId, ResolutionStatus status)
        {
            JobId = jobId;
            Status = status;
        }
        public int JobId { get; set; }
        public ResolutionStatus Status { get; set; }

    }
}