﻿namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    public class OrderJobDetail
    {
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

        [XmlElement("SSCCBarcode")]
        public string SsccBarcode { get; set; }

        [XmlElement("SkuGoodsValue")]
        public double SkuGoodsValue { get; set; }
    }
}
