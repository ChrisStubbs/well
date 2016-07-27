namespace PH.Well.Domain
{
    using System;
    using System.Xml.Serialization;
    using Enums;

    [Serializable()]
    public  class Reason : Entity<int>
    {
        [XmlIgnore]
        public int JobReasonId { get; set; }

        [XmlElement("ReasonCode")]
        public string JobDamageReasonCode { get; set; }
    }
}
