﻿namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Xml.Serialization;

    [Serializable()]
    public class OrderJobDetail
    {

        [XmlElement("LineNumber")]
        public int LineNumber { get; set; }

        [XmlElement("PHProductCode")]
        public string PhProductCode { get; set; }

        [XmlIgnore]
        public int OriginalDespatchQty { get; set; }

        //Workaround for nullable int element
        [XmlElement("OriginalDespatchQty")]
        public string OriginalDespatchQtyString
        {
            get { return OriginalDespatchQty.ToString(); }
            set
            {
                this.OriginalDespatchQty = value == string.Empty ? 0 : int.Parse(value);
            }
        }

        [XmlElement("ProdDesc")]
        public string ProdDesc { get; set; }

        [XmlElement("OrderedQty")]
        public int OrderedQty { get; set; }

        [XmlElement("UnitMeasure")]
        public string UnitMeasure { get; set; }

        [XmlElement("PHProductType")]
        public string PhProductType { get; set; }

        [XmlElement("PackSize")]
        public string PackSize { get; set; }

        [XmlElement("SingleOrOuter")]
        public string SingleOrOuter { get; set; }

        [XmlElement("SSCCBarcode")]
        public string SsccBarcode { get; set; }

        [XmlElement("SkuGoodsValue")]
        public double SkuGoodsValue { get; set; }
    }
}
