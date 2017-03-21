namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [Serializable()]
    public class JobUpdate
    {
        public JobUpdate()
        {
            this.JobDetails = new List<JobDetailUpdate>();
            this.EntityAttributes = new List<EntityAttribute>();
        }

        [XmlElement("DeliveryDate")]
        public DateTime DeliveryDate { get; set; }

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

        [XmlElement("TextField3")]
        public string CustomerRef { get; set; }

        [XmlArray("OrderJobDetails")]
        [XmlArrayItem("OrderJobDetail", typeof(JobDetailUpdate))]
        public List<JobDetailUpdate> JobDetails { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public List<EntityAttribute> EntityAttributes { get; set; }

        [XmlIgnore]
        public bool Picked
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "PICKED");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }

        [XmlIgnore]
        public int? OrdOuters
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ORDOUTERS");

                var intTry = 0;

                if (int.TryParse(attribute?.Value, out intTry))
                {
                    return intTry;
                }

                return null;
            }
        }

        [XmlIgnore]
        public int? InvOuters
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "INVOUTERS");

                var intTry = 0;

                if (int.TryParse(attribute?.Value, out intTry))
                {
                    return intTry;
                }

                return null;
            }
        }

        [XmlIgnore]
        public bool AllowSoCrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ALLOWSOCRD");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }

        [XmlIgnore]
        public string Cod
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "COD");

                if (attribute != null)
                {
                    return attribute.Value;
                }

                return string.Empty;
            }
        }

        [XmlIgnore]
        public bool AllowReOrd
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "ALLOWREORD");

                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }
    }
}
