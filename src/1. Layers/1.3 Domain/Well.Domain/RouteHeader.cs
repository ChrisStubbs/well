namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using Enums;

    [Serializable()]
    public class RouteHeader : Entity<int>
    {
        public RouteHeader()
        {
            this.Stops = new Collection<Stop>();
            this.EntityAttributes = new Collection<Attribute>();
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

        public RouteStatus RouteStatus
        {
            get { return (RouteStatus)RouteStatusId; }
            private set { RouteStatusId = (int)value; }
        }

        public int RoutesId { get; set; }

        [XmlArray("Stops")]
        [XmlArrayItem("Stop", typeof(Stop))]
        public Collection<Stop> Stops { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(Attribute))]
        public Collection<Attribute> EntityAttributes { get; set; }
    }
}
