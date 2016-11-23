namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    [Serializable()]
    public class StopUpdate
    {
        public StopUpdate()
        {
            this.OrderJobs = new Collection<JobUpdate>();
        }

        [XmlElement("CompanyID")]
        public int CompanyId { get; set; }

        [XmlElement("StartDepotCode")]
        public string StartDepotCode { get; set; }

        [XmlIgnore]
        public DateTime? OrderDate { get; set; }

        [XmlElement("RouteDate")]
        public string OrderDateFromXml
        {
            get
            {
                return this.OrderDate?.ToShortDateString() ?? "";
            }
            set
            {
                DateTime tryDate;

                if (DateTime.TryParse(value, out tryDate))
                {
                    this.OrderDate = tryDate;
                }
            }
        }

        [XmlIgnore]
        public DateTime? DeliveryDate { get; set; }

        [XmlElement("DeliveryDate")]
        public string DeliveryDateFromXml {
            get
            {
                return this.DeliveryDate?.ToShortDateString() ?? "";
            }
            set
            {
                DateTime tryDate;

                if (DateTime.TryParse(value, out tryDate))
                {
                    this.DeliveryDate = tryDate;
                }
            }
        }

        [XmlElement("TransportOrderRef")]
        public string  TransportOrderRef { get; set; }

        [XmlElement("PlannedStopNumber")]
        public string PlannedStopNumber { get; set; }

        [XmlElement("ShellActionIndicator")]
        public string ShellActionIndicator { get; set; }

        [XmlElement("CustomerShopReference")]
        public string CustomerShopReference { get; set; }

        [XmlElement("TextField5")]
        public string TextField5 { get; set; }

        [XmlElement("PaymentMethod")]
        public string PaymentMethod { get; set; }

        [XmlElement("CreditValue")]
        public string CreditValue { get; set; }

        [XmlElement("CreditText")]
        public string CreditText { get; set; }

        [XmlElement("BookTime")]
        public string BookTime { get; set; }

        [XmlElement("ActionIndicator")]
        public string ActionIndicator { get; set; }

        [XmlArray("OrderJobs")]
        [XmlArrayItem("OrderJob", typeof(JobUpdate))]
        public Collection<JobUpdate> OrderJobs { get; set; }

        [XmlElement("Account")]
        public Account Accounts { get; set; }
    }
}
