namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
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
            this.Jobs = new List<Job>();
            this.EntityAttributes = new List<EntityAttribute>();
        }

        [XmlElement("PlannedStopNumber")]
        public string PlannedStopNumber { get; set; }

        [XmlIgnore]
        public int RouteHeaderId { get; set; }

        [XmlElement("RouteHeaderId")]
        public string RouteHeaderIdXml
        {
            get
            {
                return this.RouteHeaderId.ToString();
            }
            set
            {
                int tryInt = 0;

                if (int.TryParse(value, out tryInt))
                {
                    this.RouteHeaderId = tryInt;
                }
            }
        }

        [XmlElement("TransportOrderRef")]
        public string TransportOrderReference { get; set; }

        [XmlIgnore]
        public string RouteHeaderCode { get; set; }

        [XmlIgnore]
        public string DropId { get; set; }

        [XmlIgnore]
        public string LocationId { get; set; }

        [XmlIgnore]
        public DateTime? DeliveryDate { get; set; }

        [XmlElement("TextField3")]
        public string ShellActionIndicator { get; set; }

        [XmlIgnore]
        public string AllowOvers
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ALLOWOVERS");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public string CustUnatt
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "CUSTUNATT");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public string PHUnatt
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "PHUNATT");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public int StopStatusCodeId { get; set; }

        [XmlElement("TextField1")]
        public string StopStatusCode
        {
            get
            {
                return this.StopStatusCodeId.ToString();
            }
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
            get
            {
                return this.StopPerformanceStatusCodeId.ToString();
            }
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
            get
            {
                return this.ByPassReasonId.ToString();
            }
            set
            {
                 ByPassReasonId = string.IsNullOrEmpty(value) ? (int)ByPassReasons.Notdef : (int)StringExtensions.GetValueFromDescription<ByPassReasons>(value);
            }
        }

        [XmlElement("Account")]
        public Account Account { get; set; }

        [XmlArray("Jobs")]
        [XmlArrayItem("Job", typeof(Job))]
        public List<Job> Jobs { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public List<EntityAttribute> EntityAttributes { get; set; }

        public decimal ActualPaymentCash => PaymentAmount("ACTPAYCASH");
        public decimal ActualPaymentCheque => PaymentAmount("ACTPAYCHEQ");
        public decimal ActualPaymentCard => PaymentAmount("ACTPAYCARD");
        public decimal AccountBalance => PaymentAmount("ACCBAL");

        public decimal PaymentAmount(string attributeName)
        {
            var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == attributeName);

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


        public int CleanJobs => Jobs.Count(j => j.IsClean);

        public int ExceptionJobs => Jobs.Count(j => j.IsException);
    }
}