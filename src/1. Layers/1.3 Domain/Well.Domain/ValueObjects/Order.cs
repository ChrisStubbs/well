namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    [Serializable()]
    public class Order
    {

        public Order()
        {
            this.OrderJobs = new Collection<OrderJob>();
        }

        [XmlElement("CompanyID")]
        public int CompanyId { get; set; }

        [XmlElement("StartDepotCode")]
        public string StartDepotCode { get; set; }

        [XmlIgnore]
        public DateTime OrderDate { get; set; }

        [XmlElement("RouteDate")]
        public string OrderDateString
        {
            get { return this.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.OrderDate = DateTime.Parse(value); }
        }

        [XmlIgnore]
        public DateTime DeliveryDate { get; set; }

        [XmlElement("DeliveryDate")]
        public string DeliveryDateString
        {
            get { return this.DeliveryDate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.DeliveryDate = DateTime.Parse(value); }
        }

        [XmlElement("TransportOrderRef")]
        public string  TransportOrderRef { get; set; }

        [XmlElement("SpecialInstructions")]
        public string SpecialInstructions { get; set; }

        [XmlElement("PlannedStopNumber")]
        public string PlannedStopNumber { get; set; }

        [XmlElement("SummaryNotes")]
        public string SummaryNotes { get; set; }

        [XmlElement("PlannedArriveTime")]
        public string PlannedArriveTime { get; set; }

        [XmlElement("PlannedDepartTime")]
        public string PlannedDepartTime { get; set; }

        [XmlElement("TextField1")]
        public string TextField1 { get; set; }

        [XmlElement("TextField2")]
        public string TextField2 { get; set; }

        [XmlElement("TextField3")]
        public string TextField3 { get; set; }

        [XmlElement("TextField4")]
        public string TextField4 { get; set; }

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

        //[XmlElement("SchedulingPriority")]
        [XmlIgnore]
        public int SchedulingPriority { get; set; }

        //[XmlElement("SchedulingSequence")]
        [XmlIgnore]
        public int SchedulingSequence { get; set; }

        //[XmlElement("TimeAtStop")]
        [XmlIgnore]
        public int TimeAtStop { get; set; }

        //[XmlElement("WorkTime")]
        [XmlIgnore]
        public int WorkTime { get; set; }

        //[XmlElement("LoadingTime")]
        [XmlIgnore]
        public int LoadingTime { get; set; }

        //[XmlElement("UnloadingTime")]
        [XmlIgnore]
        public int UnloadingTime { get; set; }

        //[XmlElement("Revenue")]
        [XmlIgnore]
        public double Revenue { get; set; }

        [XmlElement("ActionIndicator")]
        public string ActionIndicator { get; set; }

        [XmlElement("StartWindow")]
        public string StartWindow { get; set; }

        [XmlElement("EndWindow")]
        public string EndWindow { get; set; }

        [XmlArray("OrderJobs")]
        [XmlArrayItem("OrderJob", typeof(OrderJob))]

        public Collection<OrderJob> OrderJobs { get; set; }

        [XmlElement("Account")]
        public Account Accounts { get; set; }

    }
}
