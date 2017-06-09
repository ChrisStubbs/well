namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    public class DamageSource
    {
        [XmlElement("ReasonCode")]
        public string Code { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
