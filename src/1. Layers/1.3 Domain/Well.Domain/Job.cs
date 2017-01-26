namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Serialization;
    using Enums;
    using ValueObjects;

    [Serializable()]
    public class Job : Entity<int>
    {
        public Job()
        {
            this.JobDetails = new List<JobDetail>();
            this.EntityAttributes = new List<EntityAttribute>();
            this.EntityAttributeValues = new List<EntityAttributeValue>();
        }

        [XmlIgnore]
        public int Sequence { get; set; }

        [XmlElement("Sequence")]
        public string SequenceXml
        {
            get
            {
                return this.Sequence.ToString();
            }
            set
            {
                int tryInt = 0;

                if (int.TryParse(value, out tryInt))
                {
                    this.Sequence = tryInt;
                }
            }
        }

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

        [XmlIgnore]
        public DateTime? OrderDate { get; set; }

        [XmlElement("OrderDate")]
        public string OrderDateFromXml
        {
            get
            {
                return this.OrderDate?.ToShortDateString() ?? "";
            }
            set
            {
                var enGB = new CultureInfo("en-GB");

                DateTime tryDate;

                if (DateTime.TryParseExact(value, "dd-mm-yyyy", enGB, DateTimeStyles.None, out tryDate))
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
        [XmlIgnore]
        public int? OrdOuters
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ORDOUTERS");

                var intTry = 0;

                if (int.TryParse(attribute?.Value, out intTry))
                {
                    return intTry;
                }

                return null;
            }
        }

        /// <summary>
        /// Total invoiced outers
        /// </summary>
        [XmlIgnore]
        public int? InvOuters
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "INVOUTERS");

                var intTry = 0;

                if (int.TryParse(attribute?.Value, out intTry))
                {
                    return intTry;
                }

                return null;
            }
        }

        /// <summary>
        /// Total outers for uplift
        /// </summary>
        [XmlIgnore]
        public int? ColOuters
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "COLOUTERS");

                var intTry = 0;

                if (int.TryParse(attribute?.Value, out intTry))
                {
                    return intTry;
                }

                return null;
            }
        }

        /// <summary>
        /// Total boxes for uplift collection
        /// </summary>
        [XmlIgnore]
        public int? ColBoxes
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "COLBOXES");

                var intTry = 0;

                if (int.TryParse(attribute?.Value, out intTry))
                {
                    return intTry;
                }

                return null;
            }
        }

        [XmlIgnore]
        public bool ReCallPrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "RECALLPRD");

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
        [XmlIgnore]
        public bool AllowSoCrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ALLOWSOCRD");

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
        [XmlIgnore]
        public string Cod
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "COD");

                if (attribute != null)
                {
                    return attribute.Value;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Sandwich order
        /// </summary>
        [XmlIgnore]
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
        /// ReOrder allowed
        /// </summary>
        [XmlIgnore]
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

        [XmlElement("Reason_Description")]
        public string JobByPassReason { get; set; }

        public decimal TotalCreditValueForThreshold()
        {
            return JobDetails.Sum(d => d.CreditValueForThreshold());
        }

        [XmlIgnore]
        public int StopId { get; set; }

        [XmlArray("JobDetails")]
        [XmlArrayItem("JobDetail", typeof(JobDetail))]
        public List<JobDetail> JobDetails { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public List<EntityAttribute> EntityAttributes { get; set; }

        [XmlArray("EntityAttributeValues")]
        [XmlArrayItem("EntityAttributeValue", typeof(EntityAttributeValue))]
        public List<EntityAttributeValue> EntityAttributeValues { get; set; }

        //ACTLOGNO CUSTSRVCON DISCFOUND GRNNO GRNREFREAS ISOVERAGE OUTERCOUNT OVERORDNO TOTOVER TOTSHORT
        [XmlIgnore]
        public string ActionLogNumber
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ACTLOGNO");

                return attribute?.Value;
            }
        }

        public string GrnNumberUpdate { get; set; }

        [XmlIgnore]
        public string GrnNumber
        {
            get
            {
                var attribute = this.EntityAttributeValues.FirstOrDefault(x => x.EntityAttribute.Code == "GRNNO");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public string GrnRefusedReason
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "GRNREFREAS");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public string OuterCount
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "OUTERCOUNT");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public bool OuterDiscrepancyFound
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "DISCFOUND");

                if (attribute != null)
                  {
                      return attribute.Value != "N";
                  }

                    return false;
            }
        }

        [XmlIgnore]
        public string TotalOutersOver
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "TOTOVER");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public string TotalOutersShort
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "TOTSHORT");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public bool Picked
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "PICKED");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }

        [XmlIgnore]
        public decimal InvoiceValue
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "INVALUE");

                decimal total;
                var result = decimal.TryParse(attribute?.Value, out total);

                if (result)
                {
                    return total;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool IsException => ExceptionStatuses.Statuses.Contains(PerformanceStatus);

        public bool IsClean => PerformanceStatus == PerformanceStatus.Compl;

        public bool IsResolved => PerformanceStatus == PerformanceStatus.Resolved;
    }
}
