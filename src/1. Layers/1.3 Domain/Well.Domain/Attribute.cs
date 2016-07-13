namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable()]
    public class Attribute : Entity<int>
    {
        [XmlIgnore]
        public int AttributeId { get; set; }

        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("Value1")]
        public string Value1 { get; set; }

    }
}
