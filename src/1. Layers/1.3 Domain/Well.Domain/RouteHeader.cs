namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using Enums;
    using ValueObjects;

    [Serializable()]
    public class RouteHeader : Entity<int>
    {
        public RouteHeader()
        {
            this.Stops = new Collection<Stop>();
            this.EntityAttributes = new Collection<Attribute>();
            this.Depot = new Depot();
        }

        [XmlElement("CompanyID")]
        public int CompanyID { get; set; }

        [XmlElement("RouteNumber")]
        public string RouteNumber { get; set; }

        public DateTime RouteDate { get; set; }

        [XmlElement("RouteDate")]
        public string RouteDateString
        {
            get { return this.RouteDate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.RouteDate = DateTime.Parse(value); }
        }

        [XmlElement("DriverName")]
        public string DriverName { get; set; }

        [XmlElement("VehicleReg")]
        public string VehicleReg { get; set; }

        [XmlElement("StartDepotCode")]
        public string StartDepotCode  { get; set; }

        [XmlElement("PlannedRouteStartTime")]
        public string PlannedRouteStartTime { get; set; }

        [XmlElement("PlannedRouteFinishTime")]
        public string PlannedRouteFinishTime { get; set; }

        [XmlElement("PlannedDistance")]
        public decimal PlannedDistance { get; set; }

        [XmlElement("PlannedTravelTime")]
        public string PlannedTravelTime { get; set; }

        [XmlElement("PlannedStops")]
        public int PlannedStops { get; set; }

        public int RouteStatusId { get; set; }

        [XmlElement("RouteStatusCode")]
        public string RouteStatusCode
        {
            get { return RouteStatusCode; }
            private set { RouteStatusId = (int) (RouteStatusCode) Enum.Parse(typeof(RouteStatusCode), value); }
        }

        public int RoutePerformanceStatusId { get; set; }

        [XmlElement("PerformanceStatusCode")]
        public string PerformanceStatusCode
        {
            get { return PerformanceStatusCode; }
            private set { RoutePerformanceStatusId = (int)(PerformanceStatusCode)Enum.Parse(typeof(PerformanceStatusCode), value); }
        }

        [XmlIgnore]
        public DateTime LastRouteUpdate { get; set; }

        [XmlElement("LastRouteUpdate")]
        public string LastRouteUpdateString
        {
            get { return this.LastRouteUpdate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.LastRouteUpdate = DateTime.Parse(value); }
        }

        [XmlElement("AuthByPass")]
        public int AuthByPass { get; set; }

        [XmlElement("NonAuthByPass")]
        public int NonAuthByPass { get; set; }

        [XmlElement("ShortDeliveries")]
        public int ShortDeliveries { get; set; }

        [XmlElement("DamagesRejected")]
        public int DamagesRejected { get; set; }

        [XmlElement("DamagesAccepted")]
        public int DamagesAccepted { get; set; }

        [XmlElement("NotRequired")]
        public int NotRequired { get; set; }

        public int RoutesId { get; set; }

        [XmlElement("Depot")]
        public Depot Depot { get; set; }

        [XmlArray("Stops")]
        [XmlArrayItem("Stop", typeof(Stop))]
        public Collection<Stop> Stops { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(Attribute))]
        public Collection<Attribute> EntityAttributes { get; set; }
    }
}
