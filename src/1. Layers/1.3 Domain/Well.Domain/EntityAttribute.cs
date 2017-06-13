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

        internal static bool ParseBool(EntityAttribute att)
        {
            if (att != null && att.Value != null)
            {
                return att.Value != "N";
            }

            return false;
        }

        internal static decimal ParseDecimal(EntityAttribute att)
        {
            if (att != null)
            {
                decimal total;
                var result = decimal.TryParse(att?.Value, out total);

                return total;
            }

            return 0M;
        }
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