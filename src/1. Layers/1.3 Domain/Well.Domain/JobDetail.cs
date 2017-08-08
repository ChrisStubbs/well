namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Common.Extensions;
    using Enums;

    public class JobDetail : Entity<int>
    {
        public const int LengthOfBarcode = 18;

        public JobDetail()
        {
            this.JobDetailDamages = new List<JobDetailDamage>();
            this.Actions = new List<JobDetailAction>();
        }

        public int LineNumber { get; set; }

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

        public string PhProductCode { get; set; }

        public int OriginalDespatchQty { get; set; }

        public string ProdDesc { get; set; }

        public int OrderedQty { get; set; }

        public int DeliveredQty { get; set; }

        public int ShortQty { get; set; }

        public int ShortsActionId { get; set; }

        public DeliveryAction ShortsAction => (DeliveryAction)this.ShortsActionId;
        
        public string UnitMeasure { get; set; }

        public string PhProductType { get; set; }

        public string PackSize { get; set; }

        public string SingleOrOuter { get; set; }

        public string SSCCBarcode { get; set; }

        public double SkuGoodsValue  { get; set; }

        public decimal? NetPrice { get; set; }
        
        public int? SubOuterDamageTotal { get; set; }

        public string LineDeliveryStatus { get; set; }

        public bool IsChecked => LineDeliveryStatus == "Exception" || LineDeliveryStatus == "Delivered" || (LineDeliveryStatus == "Unknown" && ShortQty > 0) || (LineDeliveryStatus == "Unknown" && DamageQty == OriginalDespatchQty);

        public int JobId { get; set; }

        public JobDetailStatus ShortsStatus { get; set; }

        public List<JobDetailDamage> JobDetailDamages { get; set; }

        public List<JobDetailAction> Actions { get; set; }

        public int JobDetailReasonId { get; set; }

        public int JobDetailSourceId { get; set; }

        public JobDetailReason JobDetailReason { get; set; }

        public JobDetailSource JobDetailSource { get; set; }

        public bool IsHighValue { get; set; }

        public int LineItemId { get; set; }
        
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
                auditBuilder.Append($"Damages added {string.Join(", ", damages.Select(d => d.ToString()))}. ");
            }
            else if (damagesChanged && damages.Count == 0)
            {
                auditBuilder.Append(
                    $"Damages removed, old damages {string.Join(", ", originalDamages.Select(d => d.ToString()))}. ");
            }
            else if (damagesChanged)
            {
                auditBuilder.Append($"Damages changed from " +
                    $"'{string.Join(", ", originalDamages.Select(d => d.ToString()))}' to " +
                    $"'{string.Join(", ", damages.Select(d => d.ToString()))}'. ");
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

        public decimal CreditValueForThreshold()
        {
            var sumQty = this.JobDetailDamages.Sum(d => d.Qty);
            var c = (this.ShortQty + sumQty) * Convert.ToDecimal(this.SkuGoodsValue);

            return c;
        }

        public int DamageQty => JobDetailDamages.Sum(x => x.Qty);

        public bool IsClean()
        {
            return !this.Actions.Any(p => p.Quantity > 0);
        }

        public bool IsTobaccoBag()
        {
            return this.PhProductCode.Length == LengthOfBarcode;
        }
    }
}
