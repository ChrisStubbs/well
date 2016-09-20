namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using Common.Extensions;
    using Enums;

    [Serializable()]
    public class Job:Entity<int>
    {
        public Job()
        {
            this.JobDetails = new Collection<JobDetail>();
        }

        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("JobTypeCode")]
        public string JobTypeCode { get; set; }

        [XmlElement("PHAccount")]
        public string PhAccount { get; set; }

        [XmlElement("PickListRef")]
        public string PickListRef { get; set; }

        [XmlElement("InvoiceNumber")]
        public string InvoiceNumber { get; set; }

        [XmlElement("CustomerRef")]
        public string CustomerRef { get; set; }

        [XmlIgnore]
        public DateTime OrderDate { get; set; }

        [XmlElement("OrderDate")]
        public string JobDateString
        {
            get { return this.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.OrderDate = DateTime.ParseExact(value,"dd/mm/yyyy",null); }
        }

        [XmlElement("RoyaltyCode")]
        public string RoyaltyCode { get; set; }

        [XmlElement("RoyaltyCodeDesc")]
        public string RoyaltyCodeDesc { get; set; }

        [XmlElement("OrdOuters")]
        public int OrdOuters { get; set; }

        [XmlElement("InvOuters")]
        public int InvOuters { get; set; }

        [XmlElement("ColOuters")]
        public int ColOuters { get; set; }

        [XmlElement("ColBoxes")]
        public int ColBoxes { get; set; }

        [XmlElement("ReCallPrd")]
        public bool ReCallPrd { get; set; }

        [XmlElement("AllowSgCrd")]
        public bool AllowSgCrd { get; set; }

        [XmlElement("AllowSOCrd")]
        public bool AllowSoCrd { get; set; }

        [XmlElement("COD")]
        public bool Cod { get; set; }

        [XmlElement("SandwchOrd")]
        public bool SandwchOrd { get; set; }

        [XmlElement("ComdtyType")]
        public string ComdtyType { get; set; }

        [XmlElement("AllowReOrd")]
        public bool AllowReOrd { get; set; }

        [XmlElement("GrnNumber")]
        public string GrnNumber { get; set; }

        [XmlElement("GrnRefusedReason")]
        public string GrnRefusedReason { get; set; }

        [XmlElement("GrnRefusedDesc")]
        public string GrnRefusedDesc { get; set; }


        [XmlIgnore]
        public int PerformanceStatusId { get; set; }

        [XmlElement("PerformanceStatusCode")]
        public string JobPerformanceStatusCode
        {
            get { return JobPerformanceStatusCode; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    PerformanceStatusId = (int)PerformanceStatus.Notdef;
                }
                else
                {
                    PerformanceStatusId = (int)(PerformanceStatus)Enum.Parse(typeof(PerformanceStatus), value, true);
                }             
            }
        }

        [XmlIgnore]
        public int ByPassReasonId { get; set; }

        [XmlElement("Reason_Description")]
        public string JobByPassReason
        {
            get { return JobByPassReason; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ByPassReasonId = (int) ByPassReasons.Notdef;
                }
                else
                {
                    ByPassReasonId = (int)StringExtensions.GetValueFromDescription<ByPassReasons>(value);
                }            
            }
        }

        [XmlIgnore]
        public int StopId { get; set; }

        [XmlArray("JobDetails")]
        [XmlArrayItem("JobDetail", typeof(JobDetail))]
        public Collection<JobDetail> JobDetails { get; set; }

    }
}
