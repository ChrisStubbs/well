namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    public class OrderJobDetail
    {
        [XmlElement("ProdDesc")]
        public string ProdDesc { get; set; }

        [XmlElement("OrderedQty")]
        public int OrderedQty { get; set; }

        [XmlElement("SkuWeight")]
        public decimal SkuWeight { get; set; }

        [XmlElement("SkuCube")]
        public decimal SkuCube { get; set; }

        [XmlElement("UnitMeasure")]
        public string UnitMeasure { get; set; }

        [XmlElement("TextField1")]
        public string TextField1 { get; set; }

        [XmlElement("TextField2")]
        public string TextField2 { get; set; }

        [XmlElement("TextField3")]
        public string TextField3 { get; set; }

        [XmlElement("TextField4")]
        public string TextField4 { get; set; }

        [XmlElement("TextField5")]
        public string TextField5 { get; set; }

        [XmlElement("SkuGoodsValue")]
        public double SkuGoodsValue { get; set; }
    }
}
