namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using Base;
    using Enums;

    public class RouteHeader:Entity<int>
    {
        public RouteHeader()
        {
            this.RouteAttributes = new List<RouteHeaderAttribute>();
            this.Stops = new List<Stop>();
        }

        public int CompanyId { get; set; }
        public string RouteNumber { get; set; }
        public DateTime RouteDate { get; set; }
        public string DriverName { get; set; }
        public string VehicleReg { get; set; }
        public string StartDepotCode  { get; set; }
        public string StartDepotId { get; set; }
        public string FinishDepotCode { get; set; }
        public string FinishDepotId { get; set; }
        public string SubDepotCode { get; set; }
        public string SubDepotId { get; set; }
        public string FinishSubDepotCode { get; set; }
        public string FinishSubDepotId { get; set; }
        public TimeSpan PlannedRouteStartTime { get; set; }
        public TimeSpan PlannedRouteFinishTime { get; set; }
        public string InitialSealNumber { get; set; }
        public decimal PlannedDistance { get; set; }
        public TimeSpan PlannedTravelTime { get; set; }
        public int PlannedStops { get; set; }
        public RouteStatus RouteStatus { get; set; }
        public int RouteImportId { get; set; }
        public List<RouteHeaderAttribute> RouteAttributes { get; set; } 
        public List<Stop> Stops { get; set; } 
    }
}
