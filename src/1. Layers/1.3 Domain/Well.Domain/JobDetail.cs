


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


        [XmlElement("DeliveredQty")]
        public string DeliveredQty { get; set; }



       
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

        [XmlElement("NetPrice")]
        public double NetPrice { get; set; }

        [XmlElement("SubOuterDamageTotal")]
        public int SubOuterDamageTotal { get; set; }

        [XmlIgnore]
        public int JobId { get; set; }

        [XmlIgnore]
        public int JobDetailStatusId { get; set; }

       
        public decimal CreditValueForThreshold()
        {
            var sumQty = this.JobDetailDamages.Sum(d => d.Qty);
            var c = (this.ShortQty + sumQty) * Convert.ToDecimal(NetPrice);
            return c;
        }

        [XmlArray("JobDetailDamages")]
        [XmlArrayItem("JobDetailDamage", typeof(JobDetailDamage))]
        public Collection<JobDetailDamage> JobDetailDamages { get; set; }

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
                auditBuilder.Append($"Damages added {string.Join(", ", damages.Select(d => d.GetDamageString()))}. ");
            }
            else if (damagesChanged && damages.Count == 0)
            {
                auditBuilder.Append(
                    $"Damages removed, old damages {string.Join(", ", originalDamages.Select(d => d.GetDamageString()))}. ");
            }
            else if (damagesChanged)
            {
                auditBuilder.Append($"Damages changed from " +
                    $"{string.Join(", ", originalDamages.Select(d => d.GetDamageString()))} to " +
                    $"{string.Join(", ", damages.Select(d => d.GetDamageString()))}. ");
            }

            string entry = string.Empty;
            if (auditBuilder.Length > 0)
            {
                entry = $"Product: {PhProductCode} - {ProdDesc}. {auditBuilder}"; 
            }

            var audit = new Audit
            {
                Entry = entry,
                Type = AuditType.DeliveryLineUpdate,
                AccountCode = accountCode,
                InvoiceNumber = invoiceNumber,
                DeliveryDate = deliveryDate
            };

            return audit;
        }
    }
}
