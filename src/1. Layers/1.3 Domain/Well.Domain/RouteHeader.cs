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

        public DateTime? RouteDate { get; set; }

        [XmlElement("RouteDate")]
        public string RouteDateFromXml
        {
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

        public int StartDepot { get; set; }
         
        [XmlElement("PlannedStops")]
        public int PlannedStops { get; set; }

        [XmlElement("ActualStopsCompleted")]
        public int ActualStopsCompleted { get; set; }

        public RouteStatusCode RouteStatus { get; set; }

        [XmlElement("RouteStatusCode")]
        public string RouteStatusCode { get; set; }

        public int RoutePerformanceStatusId { get; set; }

        [XmlElement("RoutePerformanceStatusCode")]
        public string PerformanceStatusCode
        {
            set { RoutePerformanceStatusId = string.IsNullOrWhiteSpace(value) ? (int)RoutePerformanceStatusCode.Notdef : (int)(RoutePerformanceStatusCode)Enum.Parse(typeof(RoutePerformanceStatusCode), value, true); }
        }

        public DateTime? LastRouteUpdate { get; set; }

        [XmlElement("LastRouteUpdate")]
        public string LastRouteUpdateFromXml {
            set
            {
                DateTime tryDate;

                if (DateTime.TryParse(value, out tryDate))
                {
                    this.LastRouteUpdate = tryDate;
                }
            }
        }

        public int AuthByPass { get; set; }

        [XmlElement("AuthByPass")]
        public string AuthByPassFromXml
        {
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.AuthByPass = tryInt;
                }
            }
        }

        public int NonAuthByPass { get; set; }

        [XmlElement("NonAuthByPass")]
        public string NonAuthByPassFromXml
        {
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.NonAuthByPass = tryInt;
                }
            }
        }

        public int ShortDeliveries { get; set; }

        [XmlElement("ShortDeliveries")]
        public string ShortDeliveriesFromXml
        {
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.ShortDeliveries = tryInt;
                }
            }
        }

        public int DamagesRejected { get; set; }

        [XmlElement("DamagesRejected")]
        public string DamagesRejectedFromXml
        {
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.DamagesRejected = tryInt;
                }
            }
        }

        public int DamagesAccepted { get; set; }

        [XmlElement("DamagesAccepted")]
        public string DamagesAcceptedFromXml
        {
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.DamagesAccepted = tryInt;
                }
            }
        }

        public int NotRequired { get; set; }

        [XmlElement("NotRequired")]
        public string NotRequiredFromXml
        {
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.NotRequired = tryInt;
                }
            }
        }

        public int RoutesId { get; set; }

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
