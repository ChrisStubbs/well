namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using Common.Extensions;
    using Enums;
    using ValueObjects;

    [Serializable()]
    public class Job:Entity<int>
    {
        public Job()
        {
            this.EntityAttributes = new Collection<Attribute>();
            this.JobDetails = new Collection<JobDetail>();
        }

        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("JobTypeCode")]
        public string JobTypeCode { get; set; }

        [XmlElement("JobRef1")]
        public string JobRef1 { get; set; }

        [XmlElement("JobRef2")]
        public string JobRef2 { get; set; }

        [XmlElement("JobRef3")]
        public string JobRef3 { get; set; }

        [XmlElement("JobRef4")]
        public string JobRef4 { get; set; }

        [XmlIgnore]
        public DateTime OrderDate { get; set; }

        [XmlElement("OrderDate")]
        public string JobDateString
        {
            get { return this.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.OrderDate = DateTime.ParseExact(value,"dd/mm/yyyy",null); }
        }

        [XmlElement("Originator")]
        public string Originator { get; set; }

        [XmlElement("TextField1")]
        public string TextField1 { get; set; }

        [XmlElement("TextField2")]
        public string TextField2 { get; set; }

        [XmlIgnore]
        public PerformanceStatus PerformanceStatus { get; set; }

        [XmlElement("PerformanceStatusCode")]
        public string JobPerformanceStatusCode
        {
            get { return PerformanceStatus.ToString(); }
            set
            {
                PerformanceStatus = string.IsNullOrEmpty(value)
                    ? PerformanceStatus.Notdef
                    : (PerformanceStatus) Enum.Parse(typeof(PerformanceStatus), value, true);
            }
        }

        [XmlIgnore]
        public ByPassReasons ByPassReason { get; set; }

        [XmlElement("Reason_Description")]
        public string JobByPassReason
        {
            get { return StringExtensions.GetEnumDescription(ByPassReason); }
            set
            {
                ByPassReason = string.IsNullOrEmpty(value)
                    ? ByPassReasons.Notdef
                    : StringExtensions.GetValueFromDescription<ByPassReasons>(value);
            }
        }

        [XmlIgnore]
        public int StopId { get; set; }

        [XmlArray("JobDetails")]
        [XmlArrayItem("JobDetail", typeof(JobDetail))]
        public Collection<JobDetail> JobDetails { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(Attribute))]
        public Collection<Attribute> EntityAttributes { get; set; }

        public bool IsException => ExceptionStatuses.Statuses.Contains(PerformanceStatus);
        public bool IsClean => PerformanceStatus == PerformanceStatus.Compl;
        public bool IsResolved => PerformanceStatus == PerformanceStatus.Resolved;
    }
}
