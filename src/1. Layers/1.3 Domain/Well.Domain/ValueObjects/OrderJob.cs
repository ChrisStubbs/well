namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    [Serializable()]
    public class OrderJob
    {

        public OrderJobDetail()
        {
            this.OrderJobDetails = new Collection<OrderJobDetail>();
        }

        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("JobTypeCode")]
        public string JobTypeCode { get; set; }

        [XmlElement("JobRef1")]
        public string JobRef1 { get; set; }

        [XmlElement("JobRef2")]
        public string JobRef2 { get; set; }

        [XmlElement("JobRef3")]
        public string JobRef3 { get; set; }

        [XmlElement("JobRef4")]
        public string JobRef4 { get; set; }

        [XmlArray("OrderJobDetails")]
        [XmlArrayItem("OrderJobDetail", typeof(OrderJobDetail))]

        public Collection<OrderJobDetail> OrderJobDetails { get; set; }
    }
}
