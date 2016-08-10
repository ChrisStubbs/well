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

        [XmlElement("OrderDate")]
        public DateTime OrderDate { get; set; }

        [XmlElement("DeliveryDate")]
        public DateTime DeliveryDate { get; set; }

        [XmlElement("TransportOrderRef")]
        public string  TransportOrderRef { get; set; }

        [XmlElement("SpecialInstructions")]
        public string SpecialInstructions { get; set; }

        [XmlElement("SummaryNotes")]
        public string SummaryNotes { get; set; }

        [XmlElement("PlannedArriveTime")]
        public DateTime PlannedArriveTime { get; set; }

        [XmlElement("PlannedDepartTime")]
        public DateTime PlannedDepartTime { get; set; }

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

        [XmlElement("SchedulingPriority")]
        public int SchedulingPriority { get; set; }

        [XmlElement("SchedulingSequence")]
        public int SchedulingSequence { get; set; }

        [XmlElement("TimeAtStop")]
        public int TimeAtStop { get; set; }

        [XmlElement("WorkTime")]
        public int WorkTime { get; set; }

        [XmlElement("LoadingTime")]
        public int LoadingTime { get; set; }

        [XmlElement("UnloadingTime")]
        public int UnloadingTime { get; set; }

        [XmlElement("Revenue")]
        public double Revenue { get; set; }

        [XmlElement("ActionIndicator")]
        public string ActionIndicator { get; set; }

        [XmlElement("StartWindow")]
        public DateTime StartWindow { get; set; }

        [XmlElement("StartWindow")]
        public DateTime EndWindow { get; set; }

        [XmlArray("OrderJobs")]
        [XmlArrayItem("OrderJob", typeof(OrderJob))]

        public Collection<OrderJob> OrderJobs { get; set; }

    }
}
