namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using Common.Extensions;
    using Enums;

    [Serializable()]
    public class Stop : Entity<int>
    {
        public Stop()
        {
            this.Accounts = new Account();
            this.EntityAttributes = new Collection<Attribute>();
            this.Jobs = new Collection<Job>();
        }

        [XmlElement("PlannedStopNumber")]
        public string PlannedStopNumber { get; set; }

        [XmlElement("PlannedArriveTime")]
        public string PlannedArriveTime { get; set; }

        [XmlElement("PlannedDepartTime")]
        public string PlannedDepartTime { get; set; }

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

        [XmlElement("SpecialInstructions")]
        public string SpecialInstructions { get; set; }

        [XmlElement("StartWindow")]
        public string StartWindow { get; set; }

        [XmlElement("EndWindow")]
        public string EndWindow { get; set; }

        [XmlElement("TextField1")]
        public string TextField1 { get; set; }

        [XmlElement("TextField2")]
        public string TextField2 { get; set; }

        [XmlElement("TextField3")]
        public string TextField3 { get; set; }

        [XmlElement("TextField4")]
        public string TextField4 { get; set; }

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

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(Attribute))]
        public Collection<Attribute> EntityAttributes { get; set; }

        [XmlArray("Jobs")]
        [XmlArrayItem("Job", typeof(Job))]
        public Collection<Job> Jobs { get; set; }
    }
}