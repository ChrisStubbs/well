namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [Serializable()]
    public class JobDetailUpdate
    {
        public JobDetailUpdate()
        {
            this.EntityAttributes = new List<EntityAttribute>();
        }

        [XmlElement("LineNumber")]
        public int LineNumber { get; set; }

        [XmlElement("Barcode")]
        public string PhProductCode { get; set; }

        [XmlIgnore]
        public int OriginalDespatchQty { get; set; }

        [XmlElement("OriginalDespatchQty")]
        public string OriginalDespatchQtyFromXml
        {
            get
            {
                return this.OriginalDespatchQty.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.OriginalDespatchQty = tryInt;
                }
            }
        }

        [XmlElement("ProdDesc")]
        public string ProdDesc { get; set; }

        [XmlElement("OrderedQty")]
        public int OrderedQty { get; set; }

        [XmlElement("UnitMeasure")]
        public string UnitMeasure { get; set; }

        [XmlElement("TextField1")]
        public string PhProductType { get; set; }

        [XmlElement("TextField2")]
        public string PackSize { get; set; }

        [XmlElement("SingleOrOuter")]
        public string SingleOrOuter { get; set; }

        [XmlElement("TextField5")]
        public string SSCCBarcode { get; set; }  // tobacco bag barcode

        [XmlElement("SkuGoodsValue")]
        public double SkuGoodsValue { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public List<EntityAttribute> EntityAttributes { get; set; }

        [XmlIgnore]
        public decimal? NetPrice
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "NETPRICE");

                if (attribute != null)
                {
                    decimal value = 0M;

                    decimal.TryParse(attribute.Value, out value);

                    return value;
                }

                return null;
            }
        }
    }
}
