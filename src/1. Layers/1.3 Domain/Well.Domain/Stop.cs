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
            this.Accounts = new Account();
            this.Jobs = new Collection<Job>();
        }

        [XmlElement("PlannedStopNumber")]
        public string PlannedStopNumber { get; set; }

        [XmlIgnore]
        public int RouteHeaderId { get; set; }

        [XmlElement("TransportOrderRef")]
        public string TransportOrderRef { get; set; }

        [XmlIgnore]
        public string RouteHeaderCode { get; set; }

        [XmlIgnore]
        public string DropId { get; set; }

        [XmlIgnore]
        public string LocationId { get; set; }

        [XmlIgnore]
        public DateTime DeliveryDate { get; set; }

        [XmlElement("ShellActionIndicator")]
        public string ShellActionIndicator { get; set; }

        [XmlElement("CustomerShopReference")]
        public string CustomerShopReference { get; set; }

        [XmlElement("AllowOvers")]
        public string AllowOvers { get; set; }

        [XmlElement("CustUnatt")]
        public string CustUnatt { get; set; }

        [XmlElement("PHUnatt")]
        public string PHUnatt { get; set; }

        [XmlIgnore]
        public int StopStatusCodeId { get; set; }

        [XmlElement("StopStatusCode")]
        public string StopStatusCode
        {
            set
            {          
                StopStatusCodeId = string.IsNullOrEmpty(value) ? (int)StopStatus.Notdef : (int)(StopStatus)Enum.Parse(typeof(StopStatus), value, true);
            }
        }

        [XmlIgnore]
        public int StopPerformanceStatusCodeId { get; set; }

        [XmlElement("PerformanceStatusCode")]
        public string StopPerformanceStatusCode
        {
            set
            {
                StopPerformanceStatusCodeId = string.IsNullOrEmpty(value) ? (int)PerformanceStatus.Notdef : (int)(PerformanceStatus)Enum.Parse(typeof(PerformanceStatus), value, true);
            }
        }

        [XmlIgnore]
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
        public Account Accounts { get; set; }

        [XmlArray("Jobs")]
        [XmlArrayItem("Job", typeof(Job))]
        public Collection<Job> Jobs { get; set; }

        public int CleanJobs => Jobs.Count(j => j.IsClean);
        public int ExceptionJobs => Jobs.Count(j => j.IsException);
    }
}