namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Serialization;
    using Enums;

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

        [XmlIgnore]
        public string JobTypeAbbreviation { get; set; }

        [XmlElement("JobType_Code")]
        public string JobTypeCodeTransend { get; set; }

        public string GetJobTypeCode()
        {
            if (!string.IsNullOrWhiteSpace(this.JobTypeCode)) return this.JobTypeCode;

            if (!string.IsNullOrWhiteSpace(this.JobTypeCodeTransend)) return this.JobTypeCodeTransend;

            return "Not found";
        }

        [XmlElement("JobRef1")] // not sure we need this...neither do I
        public string SiteBunId { get; set; }

        [XmlElement("JobRef2")]
        public string PickListRef { get; set; }

        [XmlElement("JobRef3")]
        public string InvoiceNumber { get; set; }

        [XmlElement("JobRef4")]
        public string PhAccount { get; set; }

        [XmlIgnore]
        public int PhAccountId { get; set; }

        [XmlIgnore]
        public decimal CreditValue { get; set; }

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

        [XmlElement("GrnProcType")]
        public int? GrnProcessType { get; set; }

        [XmlElement("ProofDeliv")]
        public int? ProofOfDelivery { get; set; }

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
        public string PerformanceStatusCode
        {
            get
            {
                return PerformanceStatus.ToString();
            }
            set
            {
                PerformanceStatus = string.IsNullOrEmpty(value)
                    ? PerformanceStatus.Notdef
                    : (PerformanceStatus)Enum.Parse(typeof(PerformanceStatus), value, true);
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

        [XmlIgnore]
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
        public int? OuterCountUpdate { get; set; }

        [XmlIgnore]
        public int? OuterCount
        {
            get
            {
                var attribute = this.EntityAttributeValues.FirstOrDefault(x => x.EntityAttribute.Code == "OUTERCOUNT");
                if (string.IsNullOrWhiteSpace(attribute?.Value))
                {
                    return null;
                }
                int outerCount = 0;
                int.TryParse(attribute?.Value, out outerCount);
                return outerCount;
            }
        }

        [XmlIgnore]
        public bool OuterDiscrepancyUpdate { get; set; }

        [XmlIgnore]
        public bool OuterDiscrepancyFound
        {
            get
            {
                int totalShort = TotalOutersShort??0;
                int detailShort = DetailOutersShort ?? 0;
                
                var discrepancy = (totalShort - detailShort);

                if (discrepancy > 0)
                {
                    return true;
                }

                return false;
            }
        }

        [XmlIgnore]
        public int? TotalOutersOverUpdate { get; set; }

        [XmlIgnore]
        public int? TotalOutersOver
        {
            get
            {
                var attribute = this.EntityAttributeValues.FirstOrDefault(x => x.EntityAttribute.Code == "TOTOVER");
                if (string.IsNullOrWhiteSpace(attribute?.Value))
                {
                    return null;
                }
                int totalOutersOver = 0;
                int.TryParse(attribute?.Value, out totalOutersOver);
                return totalOutersOver;
            }
        }

        [XmlIgnore]
        public int? TotalOutersShortUpdate { get; set; }

        [XmlIgnore]
        public int? TotalOutersShort
        {
            get
            {
                var attribute = this.EntityAttributeValues.FirstOrDefault(x => x.EntityAttribute.Code == "TOTSHORT");
                if (string.IsNullOrWhiteSpace(attribute?.Value))
                {
                    return null;
                }
                int totalOutersShort = 0;
                int.TryParse(attribute?.Value, out totalOutersShort);
                return totalOutersShort;
            }
        }

        [XmlIgnore]
        public int? DetailOutersOverUpdate { get; set; }

        [XmlIgnore]
        public int? DetailOutersOver
        {
            get
            {
                var attribute = this.EntityAttributeValues.FirstOrDefault(x => x.EntityAttribute.Code == "DETOVER");
                   if (string.IsNullOrWhiteSpace(attribute?.Value))
                {
                    return null;
                }
                int detailOutersOver = 0;
                int.TryParse(attribute?.Value, out detailOutersOver);
                return detailOutersOver;
            }
        }

        [XmlIgnore]
        public int? DetailOutersShortUpdate { get; set; }

        [XmlIgnore]
        public int? DetailOutersShort
        {
            get
            {
                var attribute = this.EntityAttributeValues.FirstOrDefault(x => x.EntityAttribute.Code == "DETSHORT");
                if (string.IsNullOrWhiteSpace(attribute?.Value))
                {
                    return null;
                }
                int detailOutersShort = 0;
                int.TryParse(attribute?.Value, out detailOutersShort);
                return detailOutersShort;

            }
        }

        [XmlIgnore]
        public bool Picked
        {
            get
            {
                var attribute = this.EntityAttributeValues.FirstOrDefault(x => x.EntityAttribute.Code == "PICKED");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }

        [XmlIgnore]
        public decimal InvoiceValueUpdate { get; set; }

        [XmlIgnore]
        public decimal InvoiceValue
        {
            get
            {
                var attribute = this.EntityAttributeValues.FirstOrDefault(x => x.EntityAttribute.Code == "INVALUE");

                decimal total = 0;
                decimal.TryParse(attribute?.Value, out total);
                return total;
            }
        }

        [XmlIgnore]
        public JobStatus JobStatus { get; set; }

        public bool CanResolve => JobDetails.All(jd => jd.ShortsStatus == JobDetailStatus.Res &&
                                                       jd.JobDetailDamages.All(jdd => jdd.DamageStatus == JobDetailStatus.Res));

        public bool HasShorts => JobDetails.Any(x => x.ShortQty > 0);

        public bool HasDamages => this.JobDetails.SelectMany(x => x.JobDetailDamages).Sum(q => q.Qty) > 0;

        public int ToBeAdvisedCount =>  OuterDiscrepancyFound ? (TotalOutersShort.GetValueOrDefault() - DetailOutersShort.GetValueOrDefault()) : 0;
    }
}
