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

        public int CleanCount { get; set; }

        public string DriverName { get; set; }

        public string Assignee => ValueObjects.Assignee.GetDisplayNames(Assignees);

        public List<Assignee> Assignees { get; set; }

        public List<int> JobIds { get; set; }
    }

}
