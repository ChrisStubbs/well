namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    [Serializable()]
    public class JobDetail : Entity<int>
    {
        public JobDetail()
        {
           this.EntityAttributes = new Collection<Attribute>();
           this.JobDetailDamages = new Collection<JobDetailDamage>();
        }

        [XmlElement("LineNumber")]
        public int LineNumber { get; set; }

        [XmlElement("Barcode")]
        public string BarCode { get; set; }

        [XmlIgnore]
        public decimal OriginalDispatchQty { get; set; }


        [XmlElement("OriginalDespatchQty")]
        public string OriginalDispatchQtyString
        {
            get { return OriginalDispatchQtyString; }
            set
            {
                this.OriginalDispatchQty = value == string.Empty ? 0m : Convert.ToDecimal(value);
            }
        }

        [XmlElement("ProdDesc")]
        public string ProdDesc { get; set; }

        [XmlElement("OrderedQty")]
        public int OrderedQty { get; set; }

        [XmlIgnore]
        public int ShortQty { get; set; }

        [XmlElement("ShortQty")]
        public string ShortQtyString
        {
            get { return ShortQtyString; }
            set
            {
                ShortQty = value == string.Empty ? 0 : int.Parse(value);
            }
        }


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
        public double SkuGoodsValue  { get; set; }

        [XmlIgnore]
        public int JobId { get; set; }

        [XmlArray("JobDetailDamages")]
        [XmlArrayItem("JobDetailDamage", typeof(JobDetailDamage))]
        public Collection<JobDetailDamage> JobDetailDamages { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(Attribute))]
        public Collection<Attribute> EntityAttributes { get; set; }

    }
}
