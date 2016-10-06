


namespace PH.Well.Domain
{
    using System;
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
            JobDetailDamages = new Collection<JobDetailDamage>();
            Actions = new Collection<JobDetailAction>();
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

        [XmlIgnore]
        public Collection<JobDetailAction> Actions { get; set; }

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

            AuditDamages(auditBuilder, originalJobDetail.JobDetailDamages);
            AuditActions(auditBuilder, originalJobDetail.Actions);

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

        private void AuditDamages(StringBuilder auditBuilder, Collection<JobDetailDamage> originalDamages)
        {
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
                    $"'{string.Join(", ", originalDamages.Select(d => d.GetDamageString()))}' to " +
                    $"'{string.Join(", ", damages.Select(d => d.GetDamageString()))}'. ");
            }
        }

        private void AuditActions(StringBuilder auditBuilder, Collection<JobDetailAction> originalActions)
        {
            var isChanged = originalActions.Count != Actions.Count ||
                                 originalActions.OrderBy(o => o.Action).SequenceEqual(Actions.OrderBy(d => d.Action)) == false;

            if (isChanged && originalActions.Count == 0)
            {
                auditBuilder.Append($"Actions added {string.Join(", ", Actions.Select(d => d.GetString()))}. ");
            }
            else if (isChanged && Actions.Count == 0)
            {
                auditBuilder.Append(
                    $"Actions removed, old actions {string.Join(", ", originalActions.Select(d => d.GetString()))}. ");
            }
            else if (isChanged)
            {
                auditBuilder.Append($"Actions changed from " +
                    $"'{string.Join(", ", originalActions.Select(d => d.GetString()))}' to " +
                    $"'{string.Join(", ", Actions.Select(d => d.GetString()))}'. ");
            }
        }
    }
}
