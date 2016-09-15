namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Common.Extensions;
    using Enums;

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

        [XmlIgnore]
        public int JobDetailStatusId { get; set; }

        [XmlArray("JobDetailDamages")]
        [XmlArrayItem("JobDetailDamage", typeof(JobDetailDamage))]
        public Collection<JobDetailDamage>  JobDetailDamages { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(Attribute))]
        public Collection<Attribute> EntityAttributes { get; set; }

        public bool IsClean()
        {
            if (ShortQty > 0)
            {
                return false;
            }
            return JobDetailDamages.Any(d => d.Qty > 0) ? false : true;
        }

        public Audit CreateAuditEntry(JobDetail originalJobDetail, string invoiceNumber, string accountCode, DateTime deliveryDate)
        {
            var auditBuilder = new StringBuilder();

            auditBuilder.AppendConditional(originalJobDetail.ShortQty != ShortQty,
                $"Short Qty changed from {originalJobDetail.ShortQty} to {ShortQty}. ");

            var originalDamages = originalJobDetail.JobDetailDamages;
            var damages = JobDetailDamages;

            var damagesChanged = originalDamages.Count != damages.Count ||
                                 originalDamages.OrderBy(o => o.DamageReason).SequenceEqual(damages.OrderBy(d => d.DamageReason)) == false;

            if (damagesChanged && originalDamages.Count == 0)
            {
                auditBuilder.AppendConditional(true,
                    $"Damages added {string.Join(", ", damages.Select(d => d.GetDamageString()))}. ");
            }
            else if (damagesChanged && damages.Count == 0)
            {
                auditBuilder.AppendConditional(true,
                    $"Damages removed, old damages {string.Join(", ", originalDamages.Select(d => d.GetDamageString()))}. ");
            }
            else if (damagesChanged)
            {
                auditBuilder.AppendConditional(true,
                    $"Damages changed from " +
                    $"{string.Join(", ", originalDamages.Select(d => d.GetDamageString()))} to " +
                    $"{string.Join(", ", damages.Select(d => d.GetDamageString()))}. ");
            }

            var audit = new Audit
            {
                Entry = auditBuilder.ToString(),
                Type = AuditType.DeliveryLineUpdate,
                AccountCode = accountCode,
                InvoiceNumber = invoiceNumber,
                DeliveryDate = deliveryDate
            };

            return audit;
        }
    }
}
