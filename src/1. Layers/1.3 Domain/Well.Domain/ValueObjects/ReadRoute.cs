namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ReadRoute
    {
        public int Id { get; set; }

        public int BranchId { get; set; }

        public string BranchName { get; set; }

        public string Branch => $"{BranchName} ({BranchId})";

        public string Route { get; set; }

        public DateTime RouteDate { get; set; }

        public int StopCount { get; set; }

        public int RouteStatusId { get; set; }

        public string RouteStatus { get; set; }

        public int ExceptionCount { get; set; }

        public bool HasExceptions => ExceptionCount > 0;

        public string DriverName { get; set; }

        public string Assignee => string.Join(",", Assignees.Select(x => x.Name).Distinct());

        public List<ReadRouteAssignees> Assignees { get; set; }

    }

}
