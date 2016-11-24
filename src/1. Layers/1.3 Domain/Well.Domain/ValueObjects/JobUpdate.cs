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
        [XmlArrayItem("OrderJobDetail", typeof(JobDetailUpdate))]
        public List<JobDetailUpdate> JobDetails { get; set; }
    }
}
