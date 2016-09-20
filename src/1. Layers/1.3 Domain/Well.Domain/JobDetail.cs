namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Serialization;

    [Serializable()]
    public class JobDetail : Entity<int>
    {
        public JobDetail()
        {
           this.JobDetailDamages = new Collection<JobDetailDamage>();
        }

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

        [XmlIgnore]
        public int ShortQty { get; set; }

        //Workaround for nullable int element
        [XmlElement("ShortQty")]
        public string ShortQtyString
        {
            get { return ShortQty.ToString(); }
            set
            {
                ShortQty = value == string.Empty ? 0 : int.Parse(value);
            }
        }



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
        public double SkuGoodsValue  { get; set; }

        [XmlElement("SubOuterDamageTotal")]
        public int SubOuterDamageTotal { get; set; }

        [XmlIgnore]
        public int JobId { get; set; }

        [XmlIgnore]
        public int JobDetailStatusId { get; set; }

        [XmlArray("JobDetailDamages")]
        [XmlArrayItem("JobDetailDamage", typeof(JobDetailDamage))]
        public Collection<JobDetailDamage>  JobDetailDamages { get; set; }

        public bool IsClean()
        {
            if (ShortQty > 0)
            {
                return false;
            }
            return JobDetailDamages.Any(d => d.Qty > 0) ? false : true;
        }
    }
}
