namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    using PH.Well.Domain.Enums;

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

        [XmlElement("TextField5")]
        public string TextField5
        {
            get { return DropId; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    string[] text = value.Split(' ');
                    this.DropId = text[1];
                }
            }
        }

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

        [XmlElement("StopStatusCode")]
        public string StopStatusCode { get; set; }

        [XmlElement("StopStatusDescription")]
        public string StopStatusDescription { get; set; }

        [XmlElement("PerformanceStatusCode")]
        public string PerformanceStatusCode { get; set; }

        [XmlElement("PerformanceStatusDescription")]
        public string PerformanceStatusDescription { get; set; }

        [XmlElement("Reason_Description")]
        public string StopByPassReason { get; set; }

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


        public int CleanJobsCount => Jobs.Count(j => j.JobStatus == JobStatus.Clean);

        public int ExceptionJobsCount => Jobs.Count(j => j.JobStatus == JobStatus.Exception);
    }
}