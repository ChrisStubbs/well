using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace PH.Well.Domain
{
    using Enums;

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
        public int LocationId { get; set; }

        [XmlIgnore]
        public DateTime? DeliveryDate { get; set; }

        [XmlElement("TextField3")]
        public string ShellActionIndicator { get; set; }

        [XmlIgnore]
        public bool AllowOvers
        {
            get
            {
                return EntityAttribute.ParseBool(this.EntityAttributes.FirstOrDefault(x => x.Code == "ALLOWOVERS"));
            }
        }

        [XmlIgnore]
        public bool CustUnatt
        {
            get
            {
                return EntityAttribute.ParseBool(this.EntityAttributes.FirstOrDefault(x => x.Code == "CUSTUNATT"));
            }
        }

        [XmlIgnore]
        public bool PHUnatt
        {
            get
            {
                return EntityAttribute.ParseBool(this.EntityAttributes.FirstOrDefault(x => x.Code == "PHUNATT"));
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

        public decimal ActualPaymentCash => EntityAttribute.ParseDecimal(this.EntityAttributes.FirstOrDefault(x => x.Code == "ACTPAYCASH"));
        public decimal ActualPaymentCheque => EntityAttribute.ParseDecimal(this.EntityAttributes.FirstOrDefault(x => x.Code == "ACTPAYCHEQ"));
        public decimal ActualPaymentCard => EntityAttribute.ParseDecimal(this.EntityAttributes.FirstOrDefault(x => x.Code == "ACTPAYCARD"));
        public decimal AccountBalance => EntityAttribute.ParseDecimal(this.EntityAttributes.FirstOrDefault(x => x.Code == "ACCBAL"));
        
        public WellStatus WellStatus { get; set; }
     
    }
}