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
        public DateTime? RouteDate { get; set; }

        [XmlElement("RouteDate")]
        public string RouteDateFromXml
        {
            get
            {
                return this.RouteDate?.ToShortDateString() ?? "";
            }
            set
            {
                DateTime tryDate;

                if (DateTime.TryParse(value, out tryDate))
                {
                    this.RouteDate = tryDate;
                }
            }
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

        [XmlIgnore]
        public int RoutePerformanceStatusId { get; set; }

        [XmlElement("RoutePerformanceStatusCode")]
        public string PerformanceStatusCode
        {
            set { RoutePerformanceStatusId = string.IsNullOrWhiteSpace(value) ? (int)RoutePerformanceStatusCode.Notdef : (int)(RoutePerformanceStatusCode)Enum.Parse(typeof(RoutePerformanceStatusCode), value, true); }
        }

        [XmlIgnore]
        public DateTime? LastRouteUpdate { get; set; }

        [XmlElement("LastRouteUpdate")]
        public string LastRouteUpdateFromXml
        {
            get
            {
                return this.LastRouteUpdate?.ToShortDateString() ?? "";
            }
            set
            {
                DateTime tryDate;

                if (DateTime.TryParse(value, out tryDate))
                {
                    this.LastRouteUpdate = tryDate;
                }
            }
        }

        [XmlIgnore]
        public int AuthByPass { get; set; }

        [XmlElement("AuthByPass")]
        public string AuthByPassFromXml
        {
            get
            {
                return this.AuthByPass.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.AuthByPass = tryInt;
                }
            }
        }

        [XmlIgnore]
        public int NonAuthByPass { get; set; }

        [XmlElement("NonAuthByPass")]
        public string NonAuthByPassFromXml
        {
            get
            {
                return this.NonAuthByPass.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.NonAuthByPass = tryInt;
                }
            }
        }

        [XmlIgnore]
        public int ShortDeliveries { get; set; }

        [XmlElement("ShortDeliveries")]
        public string ShortDeliveriesFromXml
        {
            get
            {
                return this.ShortDeliveries.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.ShortDeliveries = tryInt;
                }
            }
        }

        [XmlIgnore]
        public int DamagesRejected { get; set; }

        [XmlElement("DamagesRejected")]
        public string DamagesRejectedFromXml
        {
            get
            {
                return this.DamagesRejected.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.DamagesRejected = tryInt;
                }
            }
        }

        [XmlIgnore]
        public int DamagesAccepted { get; set; }

        [XmlElement("DamagesAccepted")]
        public string DamagesAcceptedFromXml
        {
            get
            {
                return this.DamagesAccepted.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.DamagesAccepted = tryInt;
                }
            }
        }

        [XmlIgnore]
        public int NotRequired { get; set; }

        [XmlElement("NotRequired")]
        public string NotRequiredFromXml
        {
            get
            {
                return this.NotRequired.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.NotRequired = tryInt;
                }
            }
        }

        [XmlIgnore]
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

        [XmlIgnore]
        public object EntityAttributeValues { get; set; }

        [XmlIgnore]
        public string RouteOwner {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ROUTEOWNER");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public int RouteOwnerId { get; set; }

        public int CleanJobs => Stops.Sum(s => s.CleanJobs);

        public int ExceptionJobs => Stops.Sum(s => s.ExceptionJobs);
    }
}
