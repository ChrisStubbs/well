namespace PH.Well.Domain
{
    using System.Collections.ObjectModel;

    using PH.Well.Domain.Enums;

    public class CreditThreshold : Entity<int>
    {
        public CreditThreshold()
        {
            this.Branches = new Collection<Branch>();
        }

        public ThresholdLevel ThresholdLevel => (ThresholdLevel)this.ThresholdLevelId;

        public int ThresholdLevelId { get; set; }

        public decimal Value { get; set; }

        public Collection<Branch> Branches { get; set; }
    }
}