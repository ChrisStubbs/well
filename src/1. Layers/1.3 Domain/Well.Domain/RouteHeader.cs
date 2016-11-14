namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Serialization;
    using Enums;

    [Serializable()]
    public class RouteHeader : Entity<int>
    {
        public RouteHeader()
        {
            this.Stops = new Collection<Stop>();
            this.EntityAttributes = new Collection<EntityAttribute>();
        }

        [XmlElement("CompanyID")]
        public int CompanyId { get; set; }

        [XmlElement("StartDepotCode")]
        public string StartDepotCode { get; set; }

        [XmlIgnore]
        public DateTime RouteDate { get; set; }

        [XmlElement("RouteDate")]
        public string RouteDateString
        {
            get { return this.RouteDate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.RouteDate = DateTime.Parse(value); }
        }

        [XmlElement("RouteNumber")]
        public string RouteNumber { get; set; }

        [XmlElement("DriverName")]
        public string DriverName { get; set; }

        [XmlIgnore]
        public int StartDepot { get; set; }
         
        [XmlElement("PlannedStops")]
        public int PlannedStops { get; set; }

        [XmlElement("ActualStopsCompleted")]
        public int ActualStopsCompleted { get; set; }

        [XmlIgnore]
        public RouteStatusCode RouteStatus { get; set; }

        [XmlElement("RouteStatusCode")]
        public string RouteStatusCode { get; set; }

        public int RoutePerformanceStatusId { get; set; }

        [XmlElement("RoutePerformanceStatusCode")]
        public string PerformanceStatusCode
        {
            set { RoutePerformanceStatusId = string.IsNullOrWhiteSpace(value) ? (int)RoutePerformanceStatusCode.Notdef : (int)(RoutePerformanceStatusCode)Enum.Parse(typeof(RoutePerformanceStatusCode), value, true); }
        }

        [XmlIgnore]
        public DateTime? LastRouteUpdate { get; set; }

        [XmlElement("LastRouteUpdate")]
        public string LastRouteUpdateString
        {
            set
            {
                this.LastRouteUpdate = string.IsNullOrWhiteSpace(value) ? DateTime.Now : DateTime.Parse(value);
            }
        }

        [XmlIgnore]
        public int AuthByPass { get; set; }

        [XmlElement("AuthByPass")]
        public string AuthByPassString
        {
            get { return AuthByPassString; }
            set
            {
                this.AuthByPass = string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(value);
            }
        }

        [XmlIgnore]
        public int NonAuthByPass { get; set; }

        [XmlElement("NonAuthByPass")]
        public string NonAuthByPassString
        {
            get { return NonAuthByPassString; }
            set
            {
                this.NonAuthByPass = string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(value);
            }
        }

        [XmlIgnore]
        public int ShortDeliveries { get; set; }

        [XmlElement("ShortDeliveries")]
        public string ShortDeliveriesString
        {
            get { return ShortDeliveriesString; } 
            set
            {
                this.ShortDeliveries = value == string.Empty? 0 : int.Parse(value);
            }
        }

        [XmlIgnore]
        public int DamagesRejected { get; set; }

        [XmlElement("DamagesRejected")]
        public string DamagesRejectedString
        {
            get { return DamagesRejectedString; }
            set
            {
                this.DamagesRejected = string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(value);
            }
        }

        [XmlIgnore]
        public int DamagesAccepted { get; set; }

        [XmlElement("DamagesAccepted")]
        public string DamagesAcceptedString
        {
            get { return DamagesAcceptedString; }
            set
            {
                this.DamagesAccepted = string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(value);
            }
        }

        [XmlIgnore]
        public int NotRequired { get; set; }

        [XmlElement("NotRequired")]
        public string NotRequiredString
        {
            get { return NotRequiredString; }
            set
            {
                this.NotRequired = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            }
        }

        public int RoutesId { get; set; }

        [XmlIgnore]
        public int EpodDepot { get; set; }

        [XmlElement("Depot")]
        public string Depot { get; set; }

        [XmlArray("Stops")]
        [XmlArrayItem("Stop", typeof(Stop))]
        public Collection<Stop> Stops { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public Collection<EntityAttribute> EntityAttributes { get; set; }

        public string RouteOwner {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ROUTEOWNER");

                return attribute?.Value;
            }
        }

        public int CleanJobs => Stops.Sum(s => s.CleanJobs);

        public int ExceptionJobs => Stops.Sum(s => s.ExceptionJobs);
    }
}
