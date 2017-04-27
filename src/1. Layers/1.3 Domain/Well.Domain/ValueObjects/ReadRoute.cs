namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ReadRoute
    {
        public int Id { get; set; }

        public string BranchId { get; set; }

        public string BranchName { get; set; }

        public string Branch => $"{BranchName} ({BranchId})";

        public string Route { get; set; }

        public DateTime RouteDate { get; set; }

        public int StopCount { get; set; }

        public string RouteStatus { get; set; }

        public int ExceptionCount { get; set; }

        public string DriverName { get; set; }

        public string Assignee { get; set; }

        public List<ReadRouteAssignees> Assignees { get; set; }

    }

}
