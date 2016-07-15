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

        [XmlIgnore]
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

        [XmlElement("ActualStopsCompleted")]
        public int ActualStopsCompleted { get; set; }

        public RouteStatusCode RouteStatus { get; set; }

        [XmlElement("RouteStatusCode")]
        public string RouteStatusCode
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    RouteStatusCode enumVal;

                    if (Enum.TryParse(value, out enumVal))
                    {
                        this.RouteStatus = enumVal;
                    }
                }
            }
        }

        public int RoutePerformanceStatusId { get; set; }

        [XmlElement("RoutePerformanceStatusCode")]
        public string PerformanceStatusCode
        {
            get { return PerformanceStatusCode; }
            set { RoutePerformanceStatusId = string.IsNullOrEmpty(value) ? (int)RoutePerformanceStatusCode.Notdef : (int)(RoutePerformanceStatusCode)Enum.Parse(typeof(RoutePerformanceStatusCode), value, true); }
        }

        [XmlIgnore]
        public DateTime LastRouteUpdate { get; set; }

        [XmlElement("LastRouteUpdate")]
        public string LastRouteUpdateString
        {
            get { return this.LastRouteUpdate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set
            {             
                this.LastRouteUpdate = value == string.Empty? DateTime.Now : DateTime.Parse(value);
            }
        }

       [XmlIgnore]
        public int AuthByPass { get; set; }

        [XmlElement("AuthByPass")]
        public string AuthByPassString
        {
            get { return this.AuthByPass.ToString(); }
            set {
                this.AuthByPass = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            }
        }

        [XmlIgnore]
        public int NonAuthByPass { get; set; }

        [XmlElement("NonAuthByPass")]
        public string NonAuthByPassString
        {
            get { return this.NonAuthByPass.ToString(); }
            set {
                this.NonAuthByPass = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            }
        }

        [XmlIgnore]
        public int ShortDeliveries { get; set; }

        [XmlElement("ShortDeliveries")]
        public string ShortDeliveriesString
        {
            get { return this.ShortDeliveries.ToString(); }
            set
            {
                this.ShortDeliveries = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            }
        }

        [XmlIgnore]
        public int DamagesRejected { get; set; }

        [XmlElement("DamagesRejected")]
        public string DamagesRejectedString
        {
            get { return this.DamagesRejected.ToString(); }
            set
            {
                this.DamagesRejected = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            }
        }

        [XmlIgnore]
        public int DamagesAccepted { get; set; }

        [XmlElement("DamagesAccepted")]
        public string DamagesAcceptedString
        {
            get { return this.DamagesAccepted.ToString(); }
            set
            {
                this.DamagesAccepted = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            }
        }

        [XmlIgnore]
        public int NotRequired { get; set; }

        [XmlElement("NotRequired")]
        public string NotRequiredString
        {
            get { return this.NotRequired.ToString(); }
            set
            {
                this.NotRequired = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            }
        }


        public int RoutesId { get; set; }

        [XmlElement("Depot")]
        public string Depot { get; set; }

        [XmlArray("Stops")]
        [XmlArrayItem("Stop", typeof(Stop))]
        public Collection<Stop> Stops { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(Attribute))]
        public Collection<Attribute> EntityAttributes { get; set; }
    }
}
