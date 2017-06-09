namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;
    using PH.Well.Domain.Enums;
    using static PH.Well.Domain.EntityAttribute;

    [Serializable()]
    public class Stop : Entity<int>
    {
        public Stop()
        {
            this.Account = new Account();
            this.Jobs = new List<Job>();
        }

        public string PlannedStopNumber { get; set; }

        public int RouteHeaderId { get; set; }

        public string TransportOrderReference { get; set; }

        public string RouteHeaderCode { get; set; }

        public string DropId { get; set; }
        
        public string LocationId { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string ShellActionIndicator { get; set; }

        public bool AllowOvers { get; set; }

        public bool CustUnatt { get; set; }

        public bool PHUnatt { get; set; }

        public string StopStatusCode { get; set; }

        public string StopStatusDescription { get; set; }

        public string PerformanceStatusCode { get; set; }

        public string PerformanceStatusDescription { get; set; }

        public string StopByPassReason { get; set; }

        public Account Account { get; set; }

        public List<Job> Jobs { get; set; }

        public decimal ActualPaymentCash { get; set; }
        public decimal ActualPaymentCheque { get; set; }
        public decimal ActualPaymentCard { get; set; }
        public decimal AccountBalance { get; set; }

        public int CleanJobsCount => Jobs.Count(j => j.JobStatus == JobStatus.Clean);

        public int ExceptionJobsCount => Jobs.Count(j => j.JobStatus == JobStatus.Exception);

        public int WellStatusId { get; set; }
    }

    public class StopDTO
    {
        public StopDTO()
        {
            this.Account = new AccountDTO();
            this.Jobs = new List<JobDTO>();
            this.EntityAttributes = new List<EntityAttribute>();
        }

        [XmlIgnore]
        public int Id { get; set; }

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
        public bool AllowOvers
        {
            get
            {
                return ParseBool(this.EntityAttributes.FirstOrDefault(x => x.Code == "ALLOWOVERS"));
            }
        }

        [XmlIgnore]
        public bool CustUnatt
        {
            get
            {
                return ParseBool(this.EntityAttributes.FirstOrDefault(x => x.Code == "CUSTUNATT"));
            }
        }

        [XmlIgnore]
        public bool PHUnatt
        {
            get
            {
                return ParseBool(this.EntityAttributes.FirstOrDefault(x => x.Code == "PHUNATT"));
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
        public AccountDTO Account { get; set; }

        [XmlArray("Jobs")]
        [XmlArrayItem("Job", typeof(JobDTO))]
        public List<JobDTO> Jobs { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public List<EntityAttribute> EntityAttributes { get; set; }

        public decimal ActualPaymentCash => ParseDecimal(this.EntityAttributes.FirstOrDefault(x => x.Code == "ACTPAYCASH"));
        public decimal ActualPaymentCheque => ParseDecimal(this.EntityAttributes.FirstOrDefault(x => x.Code == "ACTPAYCHEQ"));
        public decimal ActualPaymentCard => ParseDecimal(this.EntityAttributes.FirstOrDefault(x => x.Code == "ACTPAYCARD"));
        public decimal AccountBalance => ParseDecimal(this.EntityAttributes.FirstOrDefault(x => x.Code == "ACCBAL"));
        
        public int CleanJobsCount => Jobs.Count(j => j.JobStatus == JobStatus.Clean);

        public int ExceptionJobsCount => Jobs.Count(j => j.JobStatus == JobStatus.Exception);

        public int WellStatusId { get; set; }
    }
}