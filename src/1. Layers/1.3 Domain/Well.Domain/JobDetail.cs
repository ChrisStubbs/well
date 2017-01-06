﻿namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
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
            this.JobDetailDamages = new List<JobDetailDamage>();
            this.Actions = new List<JobDetailAction>();
            this.EntityAttributes = new List<EntityAttribute>();
        }

        [XmlIgnore]
        public int LineNumber { get; set; }

        [XmlElement("LineNumber")]
        public string LineNumberXml
        {
            get
            {
                return this.LineNumber.ToString();
            }
            set
            {
                var tryInt = 0;

                if (int.TryParse(value, out tryInt))
                {
                    this.LineNumber = tryInt;
                }
            }
        }

        [XmlElement("Barcode")]
        public string PhProductCode { get; set; }

        [XmlIgnore]
        public int OriginalDespatchQty { get; set; }

        //Workaround for nullable int element
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

        [XmlIgnore]
        public int OrderedQty { get; set; }

        [XmlElement("OrderedQty")]
        public string OrderedQtyXml
        {
            get
            {
                return this.OrderedQty.ToString();
            }
            set
            {
                var tryInt = 0;

                if (int.TryParse(value, out tryInt))
                {
                    this.OrderedQty = tryInt;
                }
            }
        }

        [XmlElement("DeliveredQty")]
        public string DeliveredQty { get; set; }

        [XmlIgnore]
        public int ShortQty { get; set; }

        //Workaround for nullable int element
        [XmlElement("ShortQty")]
        public string ShortQtyFromXml
        {
            get
            {
                return this.ShortQty.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.ShortQty = tryInt;
                }
            }
        }
        
        [XmlElement("UnitMeasure")]
        public string UnitMeasure { get; set; }

        [XmlElement("TextField1")]
        public string PhProductType { get; set; }

        [XmlElement("TextField2")]
        public string PackSize { get; set; }

        [XmlElement("TextField3")]
        public string SingleOrOuter { get; set; }

        [XmlElement("TextField5")]
        public string SsccBarcode { get; set; }

        [XmlIgnore]
        public double SkuGoodsValue  { get; set; }

        [XmlElement("SkuGoodsValue")]
        public string SkuGoodsValueFromXml
        {
            get
            {
                return this.SkuGoodsValue.ToString();
            }
            set
            {
                double tryDouble;

                if (double.TryParse(value, out tryDouble))
                {
                    this.SkuGoodsValue = tryDouble;
                }
            }
        }

        [XmlIgnore]
        public string NetPrice
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "NETPRICE");

                return attribute?.Value;
            }
        }

        //[XmlElement("SubOuterDamageTotal")]
        //public int SubOuterDamageTotal { get; set; }

        [XmlIgnore]
        public string SubOuterDamageTotal
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "SUBOUTQTY");

                return attribute?.Value;
            }
        }

        public string LineDeliveryStatus
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "LINESTATUS");

                return attribute?.Value;
            }
        }

        [XmlIgnore]
        public int JobId { get; set; }

        [XmlIgnore]
        public int JobDetailStatusId { get; set; }
        
        public decimal CreditValueForThreshold()
        {
            var sumQty = this.JobDetailDamages.Sum(d => d.Qty);
            var c = (this.ShortQty + sumQty) * Convert.ToDecimal(this.SkuGoodsValue);

            return c;
        }

        [XmlArray("JobDetailDamages")]
        [XmlArrayItem("JobDetailDamage", typeof(JobDetailDamage))]
        public List<JobDetailDamage> JobDetailDamages { get; set; }

        [XmlIgnore]
        public List<JobDetailAction> Actions { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public List<EntityAttribute> EntityAttributes { get; set; }

        public int JobDetailReasonId { get; set; }

        public int JobDetailSourceId { get; set; }

        public JobDetailReason JobDetailReason { get; set; }

        public JobDetailSource JobDetailSource { get; set; }

        [XmlIgnore]
        public bool IsHighValue
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "HIGHVALUE");

                if (attribute != null)
                {
                    return attribute?.Value != "N";
                }

                return false;
            }
        }

        public bool IsClean()
        {
            if (ShortQty > 0)
            {
                return false;
            }
            return JobDetailDamages.Any(d => d.Qty > 0) ? false : true;
        }

        public Audit CreateAuditEntry(JobDetail originalJobDetail, string invoiceNumber, string accountCode, DateTime? deliveryDate)
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

        private void AuditDamages(StringBuilder auditBuilder, List<JobDetailDamage> originalDamages)
        {
            var damages = JobDetailDamages;

            var damagesChanged = originalDamages.Count != damages.Count ||
                                 originalDamages.OrderBy(o => o.JobDetailReason).SequenceEqual(damages.OrderBy(d => d.JobDetailReason)) == false;

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

        private void AuditActions(StringBuilder auditBuilder, List<JobDetailAction> originalActions)
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
