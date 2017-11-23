using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PH.Well.Common.Extensions;
using PH.Well.Domain.Enums;

namespace PH.Well.Domain
{
    public class JobDetailDTO
    {
        public JobDetailDTO()
        {
            this.JobDetailDamages = new List<JobDetailDamageDTO>();
            this.EntityAttributes = new List<EntityAttribute>();
            this.EntityAttributeValues = new List<EntityAttributeValue>();
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

        // Workaround for nullable int element
        // Invoice Quantity
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

        [XmlIgnore]
        public int DeliveredQty { get; set; }

        [XmlElement("DeliveredQty")]
        public string DeliveredQtyXml
        {
            get
            {
                return this.DeliveredQty.ToString();
            }
            set
            {
                int tryInt;

                if (int.TryParse(value, out tryInt))
                {
                    this.DeliveredQty = tryInt;
                }
            }
        }

        [XmlIgnore]
        public int ShortQty { get; set; }

        //[XmlIgnore]
        //public int ShortsActionId { get; set; }

        //[XmlIgnore]
        //public DeliveryAction ShortsAction => (DeliveryAction)this.ShortsActionId;

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
        public string SSCCBarcode { get; set; }

        [XmlIgnore]
        public double SkuGoodsValue { get; set; }

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

        //[XmlElement("SubOuterDamageTotal")]
        //public int SubOuterDamageTotal { get; set; }

        [XmlIgnore]
        public int? SubOuterDamageTotal
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "SUBOUTQTY");

                if (attribute != null)
                {
                    int value = 0;

                    int.TryParse(attribute.Value, out value);

                    return value;
                }

                return null;
            }
        }

        [XmlIgnore]
        public string LineDeliveryStatus
        {
            get
            {
                var attribute = this.EntityAttributeValues.FirstOrDefault(x => x.EntityAttribute.Code == "LINESTATUS");

                return attribute?.Value;
            }
        }

        public bool IsChecked => LineDeliveryStatus == "Exception" || LineDeliveryStatus == "Delivered";

        [XmlIgnore]
        public int JobId { get; set; }

        [XmlIgnore]
        public JobDetailStatus ShortsStatus { get; set; }

        public decimal CreditValueForThreshold()
        {
            var sumQty = this.JobDetailDamages.Sum(d => d.Qty);
            var c = (this.ShortQty + sumQty) * Convert.ToDecimal(this.SkuGoodsValue);

            return c;
        }

        [XmlArray("JobDetailDamages")]
        [XmlArrayItem("JobDetailDamage", typeof(JobDetailDamageDTO))]
        public List<JobDetailDamageDTO> JobDetailDamages { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("Attribute", typeof(EntityAttribute))]
        public List<EntityAttribute> EntityAttributes { get; set; }

        [XmlArray("EntityAttributeValues")]
        [XmlArrayItem("EntityAttributeValue", typeof(EntityAttributeValue))]
        public List<EntityAttributeValue> EntityAttributeValues { get; set; }

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

        [XmlIgnore]
        public int? UpliftAction
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "UPLIFTACTN");
                if (string.IsNullOrWhiteSpace(attribute?.Value))
                {
                    return null;
                }
                int upliftAction = 0;
                int.TryParse(attribute?.Value, out upliftAction);
                return upliftAction;
            }
        }


        [XmlIgnore]
        public bool IsSubOuterQuantity
        {
            get
            {
                var attribute = this.EntityAttributes.FirstOrDefault(x => x.Code == "SUBOUTRQTY");
                if (attribute != null)
                {
                    return attribute.Value != "N";
                }

                return false;
            }
        }

        //[XmlIgnore]
        //public int LineItemId { get; set; }

        //TODO: This will need to be moved to Line Item Actions
        //public bool IsClean()
        //{
        //    if (ShortQty > 0)
        //    {
        //        return false;
        //    }

        //    return !this.JobDetailDamages.Any(d => d.Qty > 0);
        //}

        //public Audit CreateAuditEntry(JobDetailDTO originalJobDetail, string invoiceNumber, string accountCode, DateTime? deliveryDate)
        //{
        //    var auditBuilder = new StringBuilder();

        //    auditBuilder.AppendConditional(originalJobDetail.ShortQty != ShortQty,
        //        $"Short Qty changed from {originalJobDetail.ShortQty} to {ShortQty}. ");

        //    AuditDamages(auditBuilder, originalJobDetail.JobDetailDamages);
        //    AuditActions(auditBuilder, originalJobDetail.Actions);

        //    string entry = string.Empty;
        //    if (auditBuilder.Length > 0)
        //    {
        //        entry = $"Product: {PhProductCode} - {ProdDesc}. {auditBuilder}";
        //    }

        //    var audit = new Audit
        //    {
        //        Entry = entry,
        //        Type = AuditType.DeliveryLineUpdate,
        //        AccountCode = accountCode,
        //        InvoiceNumber = invoiceNumber,
        //        DeliveryDate = deliveryDate
        //    };

        //    return audit;
        //}

        //private void AuditDamages(StringBuilder auditBuilder, List<JobDetailDamageDTO> originalDamages)
        //{
        //    var damages = JobDetailDamages;

        //    var damagesChanged = originalDamages.Count != damages.Count ||
        //                         originalDamages
        //                             .OrderBy(o => o.JobDetailReason)
        //                             .SequenceEqual(damages.OrderBy(d => d.JobDetailReason)) == false;

        //    if (damagesChanged && originalDamages.Count == 0)
        //    {
        //        auditBuilder.Append($"Damages added {string.Join(", ", damages.Select(d => d.ToString()))}. ");
        //    }
        //    else if (damagesChanged && damages.Count == 0)
        //    {
        //        auditBuilder.Append(
        //            $"Damages removed, old damages {string.Join(", ", originalDamages.Select(d => d.ToString()))}. ");
        //    }
        //    else if (damagesChanged)
        //    {
        //        auditBuilder.Append($"Damages changed from " +
        //                            $"'{string.Join(", ", originalDamages.Select(d => d.ToString()))}' to " +
        //                            $"'{string.Join(", ", damages.Select(d => d.ToString()))}'. ");
        //    }
        //}

        //private void AuditActions(StringBuilder auditBuilder, List<JobDetailAction> originalActions)
        //{
        //    var isChanged = originalActions.Count != Actions.Count ||
        //                    originalActions.OrderBy(o => o.Action).SequenceEqual(Actions.OrderBy(d => d.Action)) == false;

        //    if (isChanged && originalActions.Count == 0)
        //    {
        //        auditBuilder.Append($"Actions added {string.Join(", ", Actions.Select(d => d.GetString()))}. ");
        //    }
        //    else if (isChanged && Actions.Count == 0)
        //    {
        //        auditBuilder.Append(
        //            $"Actions removed, old actions {string.Join(", ", originalActions.Select(d => d.GetString()))}. ");
        //    }
        //    else if (isChanged)
        //    {
        //        auditBuilder.Append($"Actions changed from " +
        //                            $"'{string.Join(", ", originalActions.Select(d => d.GetString()))}' to " +
        //                            $"'{string.Join(", ", Actions.Select(d => d.GetString()))}'. ");
        //    }
        //}
    }
}