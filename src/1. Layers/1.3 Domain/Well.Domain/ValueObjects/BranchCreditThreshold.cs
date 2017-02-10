namespace PH.Well.Domain.ValueObjects
{
    using Enums;

    public class BranchCreditThreshold
    {
            public int Id { get; set; }

            public ThresholdLevel ThresholdLevel => (ThresholdLevel)this.ThresholdLevelId;

            public int ThresholdLevelId { get; set; }

            public decimal Threshold { get; set; }

            public int BranchId { get; set; }
    }
}
