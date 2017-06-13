namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;
    using Enums;

    [Serializable()]
    public class RouteHeader : Entity<int>
    {
        public RouteHeader()
        {
            this.Stops = new List<StopDTO>();
            this.EntityAttributes = new List<EntityAttribute>();
        }

        [XmlIgnore]
        public int CompanyId { get; set; }

        [XmlElement("CompanyID")]
        public string CompanyIdXml
        {
            get
            {
                return this.CompanyId.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.CompanyId = tryInt;
                }
            }
        }

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
        public int StartDepot
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.StartDepotCode))
                {
                    return (int)Branches.NotDefined;
                }

                return (int)Enum.Parse(typeof(Branches), this.StartDepotCode, true);
            }
        }

        [XmlIgnore]
        public int PlannedStops { get; set; }

        [XmlElement("PlannedStops")]
        public string PlannedStopsXml
        {
            get
            {
                return this.PlannedStops.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.PlannedStops = tryInt;
                }
            }
        }

        [XmlIgnore]
        public int ActualStopsCompleted { get; set; }

        [XmlElement("ActualStopsCompleted")]
        public string ActualStopsCompletedXml
        {
            get
            {
                return this.ActualStopsCompleted.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.ActualStopsCompleted = tryInt;
                }
            }
        }

        [XmlElement("RouteStatusDescription")]
        public string RouteStatusDescription { get; set; }

        [XmlElement("RouteStatusCode")]
        public string RouteStatusCode { get; set; }

        [XmlElement("PerformanceStatusCode")]
        public string PerformanceStatusCode { get; set; }

        [XmlElement("PerformanceStatusDescription")]
        public string PerformanceStatusDescription { get; set; }

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
        public int RoutesId { get; set; }

        [XmlArray("Stops")]
        [XmlArrayItem("Stop", typeof(StopDTO))]
        public List<StopDTO> Stops { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public List<EntityAttribute> EntityAttributes { get; set; }

        [XmlIgnore]
        public object EntityAttributeValues { get; set; }

        [XmlIgnore]
        public string RouteOwner
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ROUTEOWNER");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public int RouteOwnerId { get; set; }

        [XmlIgnore]
        public int TotalDrops { get; set; }
        
        public bool TryParseBranchIdFromRouteNumber(out int branchId )
        {
            return int.TryParse(RouteNumber.Substring(0, 2), out branchId);
        }

    }
}
