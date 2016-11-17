namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Serialization;
    using Common.Extensions;
    using Enums;

    [Serializable()]
    public class Stop : Entity<int>
    {
        public Stop()
        {
            this.Account = new Account();
            this.Jobs = new Collection<Job>();
        }

        [XmlElement("PlannedStopNumber")]
        public string PlannedStopNumber { get; set; }

        public int RouteHeaderId { get; set; }

        [XmlElement("TransportOrderRef")]
        public string TransportOrderReference { get; set; }

        public string RouteHeaderCode { get; set; }

        public string DropId { get; set; }

        public string LocationId { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [XmlElement("TextField3")]
        public string ShellActionIndicator { get; set; }

        [XmlElement("TextField4")]
        public string CustomerShopReference { get; set; }

        public string AllowOvers
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ALLOWOVERS");

                return attribute?.Value;
            }
        }

        public string CustUnatt
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "CUSTUNATT");

                return attribute?.Value;
            }
        }

        public string PHUnatt
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "PHUNATT");

                return attribute?.Value;
            }
        }

        public int StopStatusCodeId { get; set; }

        [XmlElement("TextField1")]
        public string StopStatusCode
        {
            set
            {          
                StopStatusCodeId = string.IsNullOrEmpty(value) ? (int)StopStatus.Notdef : (int)(StopStatus)Enum.Parse(typeof(StopStatus), value, true);
            }
        }

        public int StopPerformanceStatusCodeId { get; set; }

        [XmlElement("PerformanceStatusCode")]
        public string StopPerformanceStatusCode
        {
            set
            {
                StopPerformanceStatusCodeId = string.IsNullOrEmpty(value) ? (int)PerformanceStatus.Notdef : (int)(PerformanceStatus)Enum.Parse(typeof(PerformanceStatus), value, true);
            }
        }

        public int ByPassReasonId { get; set; }

        [XmlElement("Reason_Description")]
        public string StopByPassReason
        {
            get { return StopByPassReason; }
            set
            {
                 ByPassReasonId = string.IsNullOrEmpty(value) ? (int)ByPassReasons.Notdef : (int)StringExtensions.GetValueFromDescription<ByPassReasons>(value);
            }
        }

        [XmlElement("Account")]
        public Account Account { get; set; }

        [XmlArray("Jobs")]
        [XmlArrayItem("Job", typeof(Job))]
        public Collection<Job> Jobs { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public Collection<EntityAttribute> EntityAttributes { get; set; }

        public int CleanJobs => Jobs.Count(j => j.IsClean);

        public int ExceptionJobs => Jobs.Count(j => j.IsException);
    }
}