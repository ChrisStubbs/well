﻿namespace PH.Well.Api.Models
{
    using System;

    public class RouteModel
    {
        public string Branch { get; set; }

        public string Route { get; set; }

        public DateTime RouteDate { get; set; }

        public int StopCount { get; set; }

        public string RouteStatus { get; set; }

        public int ExceptionCount { get; set; }

        public string DriverName { get; set; }

        public string Assignee { get; set; }
    }
}