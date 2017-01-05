namespace PH.Well.Domain
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    public class EntityAttribute
    {
        public EntityAttribute()
        {
        }

        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("Value1")]
        public string Value { get; set; }
    }
}