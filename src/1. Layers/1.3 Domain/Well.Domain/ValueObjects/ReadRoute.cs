﻿namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ReadRoute
    {
        public ReadRoute()
        {
            JobIds = new List<int>();
        }

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

        public string Assignee
        {
            get
            {
                return Assignees.Any() ? string.Join(",", Assignees.Select(x => x.Name).Distinct()) : "Unallocated";
            }
        }

        public List<ReadRouteAssignees> Assignees { get; set; }

        public List<int> JobIds { get; set; }
    }

}
