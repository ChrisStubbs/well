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

        [XmlElement("JobRef1")]                 // not sure we need this
        public string SiteBunId { get; set; }

        [XmlElement("JobRef2")]
        public string PickListRef { get; set; }

        [XmlElement("JobRef3")]
        public string InvoiceNumber { get; set; }

        [XmlElement("JobRef4")]
        public string PhAccount { get; set; }

        public DateTime? OrderDate { get; set; }

        [XmlElement("OrderDate")]
        public string OrderDateFromXml {
            set
            {
                DateTime tryDate;

                if (DateTime.TryParse(value, out tryDate))
                {
                    this.OrderDate = tryDate;
                }
            }
        }

        [XmlElement("TextField1")]
        public string RoyaltyCode { get; set; }

        [XmlElement("TextField2")]
        public string RoyaltyCodeDesc { get; set; }

        [XmlElement("TextField3")]
        public string CustomerRef { get; set; }

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

        public bool ReCallPrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ReCallPrd");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }

        /// <summary>
        /// Single credit allowed
        /// </summary>
        public bool AllowSgCrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "AllowSgCrd");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }

        /// <summary>
        /// Sub outer credit allowed
        /// </summary>
        public bool AllowSoCrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "AllowSoCrd");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }

        /// <summary>
        /// Cash on delivery
        /// </summary>
        public bool Cod
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "COD");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }

        /// <summary>
        /// Sandwich order
        /// </summary>
        public bool SandwchOrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "SandwchOrd");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
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
        public bool AllowReOrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "AllowReOrd");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }

        //[XmlElement("GrnNumber")]
        //public string GrnNumber { get; set; }

        //[XmlElement("GrnRefusedReason")]
        //public string GrnRefusedReason { get; set; }

        [XmlElement("GrnRefusedDesc")]
        public string GrnRefusedDesc { get; set; }

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

        public int StopId { get; set; }

        [XmlArray("JobDetails")]
        [XmlArrayItem("JobDetail", typeof(JobDetail))]
        public Collection<JobDetail> JobDetails { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public Collection<EntityAttribute> EntityAttributes { get; set; }

        //ACTLOGNO CUSTSRVCON DISCFOUND GRNNO GRNREFREAS ISOVERAGE OUTERCOUNT OVERORDNO TOTOVER TOTSHORT
        public string ActionLogNumber
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ACTLOGNO");

                return attribute?.Value;
            }
        }

        public string GrnNumber
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "GRNNO");

                return attribute?.Value;
            }
        }

        public string GrnRefusedReason
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "GRNREFREAS");

                return attribute?.Value;
            }
        }

        public string OuterCount
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "OUTERCOUNT");

                return attribute?.Value;
            }
        }

        public string OuterDiscrepancyFound
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "DISCFOUND");

                return attribute?.Value;
            }
        }

        public string TotalOutersOver
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "TOTOVER");

                return attribute?.Value;
            }
        }

        public string TotalOutersShort
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "TOTSHORT");

                return attribute?.Value;
            }
        }

        public bool IsException => ExceptionStatuses.Statuses.Contains(PerformanceStatus);

        public bool IsClean => PerformanceStatus == PerformanceStatus.Compl;

        public bool IsResolved => PerformanceStatus == PerformanceStatus.Resolved;
    }
}
