namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Serialization;
    using Common.Extensions;
    using Enums;
    using ValueObjects;

    [Serializable()]
    public class Job : Entity<int>
    {
        public Job()
        {
            this.JobDetails = new Collection<JobDetail>();
        }

        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("JobTypeCode")]
        public string JobTypeCode { get; set; }

        [XmlElement("JobRef1")]
        public string PhAccount { get; set; }

        [XmlElement("PickListRef")]
        public string PickListRef { get; set; }

        [XmlElement("JobRef3")]
        public string InvoiceNumber { get; set; }

        [XmlElement("JobRef4")]
        public string CustomerRef { get; set; }

        [XmlIgnore]
        public DateTime OrderDate { get; set; }

        [XmlElement("OrderDate")]
        public string JobDateString
        {
            get { return this.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.OrderDate = DateTime.ParseExact(value,"dd-mm-yyyy",null); }
        }

        [XmlElement("TextField1")]
        public string RoyaltyCode { get; set; }

        [XmlElement("TextField2")]
        public string RoyaltyCodeDesc { get; set; }

        /// <summary>
        /// Total ordered outers
        /// </summary>
        public string OrdOuters
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ORDOUTERS");

                return attribute?.Value;
            }
        }

        /// <summary>
        /// Total invoiced outers
        /// </summary>
        public string InvOuters
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "INVOUTERS");

                return attribute?.Value;
            }
        }

        /// <summary>
        /// Total outers for uplift
        /// </summary>
        public string ColOuters
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "COLOUTERS");

                return attribute?.Value;
            }
        }

        /// <summary>
        /// Total boxes for uplift collection
        /// </summary>
        public string ColBoxes
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "COLBOXES");

                return attribute?.Value;
            }
        }

        public string ReCallPrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ReCallPrd");

                return attribute?.Value;
            }
        }

        /// <summary>
        /// Single credit allowed
        /// </summary>
        public string AllowSgCrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "AllowSgCrd");

                return attribute?.Value;
            }
        }

        /// <summary>
        /// Sub outer credit allowed
        /// </summary>
        public string AllowSoCrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "AllowSoCrd");

                return attribute?.Value;
            }
        }

        /// <summary>
        /// Cash on delivery
        /// </summary>
        public string Cod
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "COD");

                return attribute?.Value;
            }
        }

        /// <summary>
        /// Sandwich order
        /// </summary>
        public string SandwchOrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "SandwchOrd");

                return attribute?.Value;
            }
        }

        /// <summary>
        /// Commodity type
        /// </summary>
        public string ComdtyType
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ComdtyType");

                return attribute?.Value;
            }
        }

        /// <summary>
        /// ReOrder allowed
        /// </summary>
        public string AllowReOrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "AllowReOrd");

                return attribute?.Value;
            }
        }

        [XmlElement("GrnNumber")]
        public string GrnNumber { get; set; }

        [XmlElement("GrnRefusedReason")]
        public string GrnRefusedReason { get; set; }

        [XmlElement("GrnRefusedDesc")]
        public string GrnRefusedDesc { get; set; }


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

        public decimal TotalCreditValueForThreshold()
        {
            return JobDetails.Sum(d => d.CreditValueForThreshold());
        }

        [XmlIgnore]
        public int StopId { get; set; }

        [XmlArray("JobDetails")]
        [XmlArrayItem("JobDetail", typeof(JobDetail))]
        public Collection<JobDetail> JobDetails { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public Collection<EntityAttribute> EntityAttributes { get; set; }

        public bool IsException => ExceptionStatuses.Statuses.Contains(PerformanceStatus);
        public bool IsClean => PerformanceStatus == PerformanceStatus.Compl;
        public bool IsResolved => PerformanceStatus == PerformanceStatus.Resolved;
    }
}
