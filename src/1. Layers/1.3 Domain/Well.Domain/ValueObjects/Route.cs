using PH.Well.Domain.Enums;

namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;

    public class Route
    {
        public Route()
        {
            JobIds = new List<int>();
            Assignees = new List<Assignee>();
        }

        public int Id { get; set; }

        public int BranchId { get; set; }

        public string BranchName { get; set; }

        public string Branch => Domain.Branch.GetBranchName(BranchId, BranchName);

        public string RouteNumber { get; set; }

        public DateTime RouteDate { get; set; }

        public int StopCount { get; set; }

        public int RouteStatusId { get; set; }

        public string RouteStatus { get; set; }

        public int ExceptionCount { get; set; }

        public bool HasExceptions => ExceptionCount > 0;

        private int _cleanCount;
        public int CleanCount
        {
            // If it's planned shouldn't have any cleans
            get { return (RouteStatusId != (int) WellStatus.Planned) ? _cleanCount : 0; }
            set { _cleanCount = value; }
        }

        public bool HasClean => CleanCount > 0;

        public string DriverName { get; set; }

        public string Assignee => ValueObjects.Assignee.GetDisplayNames(Assignees);

        public List<Assignee> Assignees { get; set; }

        public List<int> JobIds { get; set; }

        public bool HasNotDefinedDeliveryAction { get; set; }

        public bool NoGRNButNeeds { get; set; }

        public bool PendingSubmission { get; set; }

        public JobIssueType JobIssueType { get; set; }
    }
}
