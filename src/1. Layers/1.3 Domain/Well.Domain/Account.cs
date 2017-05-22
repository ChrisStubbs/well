namespace PH.Well.Domain
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    public class Account : Entity<int>
    {
        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("AccountTypeCode")]
        public string AccountTypeCode { get; set; }

        [XmlElement("DepotID")]
        public int DepotId { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Address1")]
        public string Address1 { get; set; }

        [XmlElement("Address2")]
        public string Address2 { get; set; }

        [XmlElement("PostCode")]
        public string PostCode { get; set; }

        [XmlElement("ContactName")]
        public string ContactName { get; set; }

        [XmlElement("ContactNumber")]
        public string ContactNumber { get; set; }

        [XmlElement("ContactNumber2")]
        public string ContactNumber2 { get; set; }

        [XmlElement("ContactEmailAddress")]
        public string ContactEmailAddress { get; set; }

        [XmlIgnore]
        public int StopId { get; set; }
        }
    }

