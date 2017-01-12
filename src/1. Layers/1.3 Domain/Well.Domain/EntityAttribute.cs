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

    [Serializable()]
    public class EntityAttributeValue
    {
        public EntityAttributeValue()
        {
            this.EntityAttribute = new EntityAttribute();
        }

        [XmlElement("Value1")]
        public string Value { get; set; }

        public EntityAttribute EntityAttribute { get; set; }
    }
}