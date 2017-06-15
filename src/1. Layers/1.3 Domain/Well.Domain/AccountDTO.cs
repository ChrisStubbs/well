using System;
using System.Xml.Serialization;

namespace PH.Well.Domain
{
    [Serializable()]
    public class AccountDTO
    {
        public int Id { get; set; }

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

        [XmlIgnore]
        public DateTime? DateDeleted { get; set; }

        [XmlIgnore]
        public bool IsDeleted => DateDeleted.HasValue;
    }
}
