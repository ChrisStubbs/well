namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    [Serializable()]
    public class Reason
    {
        [XmlElement("ReasonCode")]
        public string Code { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
