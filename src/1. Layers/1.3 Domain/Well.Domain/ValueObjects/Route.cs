namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Route
    {
        public Route()
        {
            JobIds = new List<int>();
            Assignees = new List<RouteAssignees>();
        }

        public int Id { get; set; }

        public int BranchId { get; set; }

        public string BranchName { get; set; }

        public string Branch => $"{BranchName} ({BranchId})";

        public string RouteNumber { get; set; }

        public DateTime RouteDate { get; set; }

        public int StopCount { get; set; }

        public int RouteStatusId { get; set; }

        public string RouteStatus { get; set; }

        public int ExceptionCount { get; set; }

        public bool HasExceptions => ExceptionCount > 0;

        public int CleanCount { get; set; }

        public string DriverName { get; set; }

        public string Assignee
        {
            get
            {
                if (!Assignees.Any()) return "Unallocated";

                if (Assignees.Count == 1)
                {
                    return Assignees[0].Name;
                }
                   
                var initials = Assignees.Select(x => string.Join("",x.Name.Split(' ').Select(s=> s[0]))).Distinct();
                return string.Join(", ", initials);
            }
        }

        public List<RouteAssignees> Assignees { get; set; }

        public List<int> JobIds { get; set; }
    }

}
