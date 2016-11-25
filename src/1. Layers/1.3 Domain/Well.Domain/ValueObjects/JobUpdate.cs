namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable()]
    public class JobUpdate
    {
        public JobUpdate()
        {
            this.JobDetails = new List<JobDetailUpdate>();
        }

        [XmlElement("DeliveryDate")]
        public DateTime DeliveryDate { get; set; }

        [XmlElement("TextField5")]
        public string RouteNumberAndDropNumber { get; set; }

        public string RouteNumber => this.RouteNumberAndDropNumber.Split(' ')[0];

        public string DropNumber => this.RouteNumberAndDropNumber.Split(' ')[1];

        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("JobTypeCode")]
        public string JobTypeCode { get; set; }

        [XmlElement("JobRef4")]
        public string PhAccount { get; set; }

        [XmlElement("JobRef2")]
        public string PickListRef { get; set; }

        [XmlElement("JobRef3")]
        public string InvoiceNumber { get; set; }

        [XmlElement("TextField1")]
        public string CustomerRef { get; set; }

        [XmlArray("OrderJobDetails")]
        [XmlArrayItem("OrderJobDetail", typeof(JobDetailUpdate))]
        public List<JobDetailUpdate> JobDetails { get; set; }
    }
}
