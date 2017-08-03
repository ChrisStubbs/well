using PH.Well.Domain.Extensions;

namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Enums;
    using ValueObjects;

    [Serializable()]
    public class Job : Entity<int>
    {
        public const string DocumentPickListReference = "9999999";
        public Job()
        {
            JobDetails = new List<JobDetail>();
            LineItems = new List<LineItem>();
            ResolutionStatusHistory = new List<JobResolutionStatus>();
        }

        public int Sequence { get; set; }

        public string JobTypeCode { get; set; }

        public string JobType { get; set; }

        /// <summary>
        /// Helper property to get jobType as enum value
        /// Current JobType property should be renamed to JobTypeDescription
        /// JobTypeEnumValue should be then called JobType and mapped from JobType database value
        /// </summary>
        public JobType JobTypeEnumValue { get { return EnumExtensions.GetValueFromDescription<JobType>(JobTypeCode); } }

        public string JobTypeAbbreviation { get; set; }

        public string PickListRef { get; set; }

        public string InvoiceNumber { get; set; }

        public string PhAccount { get; set; }

        public int PhAccountId { get; set; }

        public string PhAccountName { get; set; }

        public decimal CreditValue { get; set; }

        public DateTime? OrderDate { get; set; }

        public string RoyaltyCode { get; set; }

        public string RoyaltyCodeDesc { get; set; }

        public string CustomerRef { get; set; }

        public int? GrnProcessType { get; set; }

        public int? ProofOfDelivery { get; set; }

        public bool IsProofOfDelivery
        {
            get
            {
                return ProofOfDelivery.HasValue && (ProofOfDelivery == (int)Enums.ProofOfDelivery.Lucozade ||
                                                    ProofOfDelivery == (int)Enums.ProofOfDelivery.CocaCola);
            }
        }

        public int? OrdOuters { get; set; }

        public int? InvOuters { get; set; }

        public int? ColOuters { get; set; }

        public int? ColBoxes { get; set; }

        public bool ReCallPrd { get; set; }

        public bool AllowSoCrd { get; set; }

        public string Cod { get; set; }

        public bool SandwchOrd { get; set; }

        public bool AllowReOrd { get; set; }

        public PerformanceStatus PerformanceStatus { get; set; }

        public string JobByPassReason { get; set; }

        public int StopId { get; set; }

        public List<JobDetail> JobDetails { get; set; }

        public string ActionLogNumber { get; set; }

        public string GrnNumberUpdate { get; set; }

        public string GrnNumber { get; set; }

        public bool IsGrnNumberRequired
        {
            get { return !string.IsNullOrWhiteSpace(GrnNumber) && GrnProcessType == 1; }
        }

        public string GrnRefusedReason { get; set; }

        public int? OuterCountUpdate { get; set; }

        public int? OuterCount { get; set; }

        public bool OuterDiscrepancyUpdate { get; set; }

        public bool OuterDiscrepancyFound { get; set; }

        public int? TotalOutersOverUpdate { get; set; }

        public int? TotalOutersOver { get; set; }

        public int? TotalOutersShortUpdate { get; set; }

        public int? TotalOutersShort { get; set; }

        public int? DetailOutersOverUpdate { get; set; }

        public int? DetailOutersOver { get; set; }

        public int? DetailOutersShortUpdate { get; set; }

        public int? DetailOutersShort { get; set; }

        public bool Picked { get; set; }

        public decimal InvoiceValueUpdate { get; set; }

        public decimal InvoiceValue { get; set; }

        public JobStatus JobStatus { get; set; }

        public bool CanResolve => JobDetails.All(jd => jd.ShortsStatus == JobDetailStatus.Res &&
                                                       jd.JobDetailDamages.All(jdd => jdd.DamageStatus == JobDetailStatus.Res));

        public bool HasShorts => JobDetails.Any(x => x.ShortQty > 0);

        public bool HasDamages => this.JobDetails.SelectMany(x => x.JobDetailDamages).Sum(q => q.Qty) > 0;

        // detail outers short & OuterDiscrepancyFound is calculated and updated after import from transend
        public int ToBeAdvisedCount => OuterDiscrepancyFound ? (TotalOutersShort.GetValueOrDefault() - DetailOutersShort.GetValueOrDefault()) : 0;

        public WellStatus WellStatus { get; set; }

        public ResolutionStatus ResolutionStatus { get; set; }

        public IList<LineItem> LineItems { get; set; }

        public IList<LineItemAction> GetAllLineItemActions()
        {
            return LineItems.SelectMany(x => x.LineItemActions).ToList();
        }

        public decimal TotalCreditValue => LineItems.Sum(x => x.TotalCreditValue);

        public decimal TotalActionValue => LineItems.Sum(x => x.TotalActionValue);

        public int TotalCreditQty => LineItems.Sum(x => x.TotalCreditQty);

        public int TotalQty => LineItems.Sum(x => x.TotalQty);

        public JobRoute JobRoute { get; set; }

        public IEnumerable<JobResolutionStatus> ResolutionStatusHistory;

        public int GetRoyaltyCode()
        {
            if (!string.IsNullOrWhiteSpace(RoyaltyCode))
            {
                var royaltyParts = RoyaltyCode.Split(' ');
                int tryParseCode;
                if (int.TryParse(royaltyParts[0], out tryParseCode))
                {
                    return tryParseCode;
                }
            }
            return default(int);

        }
    }
}
