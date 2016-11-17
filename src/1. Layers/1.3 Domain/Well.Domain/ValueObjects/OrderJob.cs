namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    [Serializable()]
    public class OrderJob
    {
        public OrderJob()
        {
            this.OrderJobDetails = new Collection<OrderJobDetail>();
        }

        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("JobTypeCode")]
        public string JobTypeCode { get; set; }

        [XmlElement("JobRef1")]
        public string PhAccount { get; set; }

        [XmlElement("JobRef2")]
        public string PickListRef { get; set; }

        [XmlElement("TextField2")]
        public string InvoiceNumber { get; set; }

        [XmlElement("TextField1")]
        public string CustomerRef { get; set; }

        [XmlArray("OrderJobDetails")]
        [XmlArrayItem("OrderJobDetail", typeof(OrderJobDetail))]
        public Collection<OrderJobDetail> OrderJobDetails { get; set; }
    }
}
