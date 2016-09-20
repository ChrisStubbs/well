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

        [XmlElement("PHAccount")]
        public string PhAccount { get; set; }

        [XmlElement("PickListRef")]
        public string PickListRef { get; set; }

        [XmlElement("InvoiceNumber")]
        public string InvoiceNumber { get; set; }

        [XmlElement("CustomerRef")]
        public string CustomerRef { get; set; }

        [XmlArray("OrderJobDetails")]
        [XmlArrayItem("OrderJobDetail", typeof(OrderJobDetail))]

        public Collection<OrderJobDetail> OrderJobDetails { get; set; }
    }
}
