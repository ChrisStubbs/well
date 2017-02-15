namespace PH.Well.Domain
{
    using System.Collections.Generic;
    using ValueObjects;

    public class Branch : Entity<int>
    {
        public Branch()
        {
            CreditThresholds = new List<BranchCreditThreshold>();
        }

        public string Name { get; set; }

        public int PreferenceId { get; set; }

        public IList<BranchCreditThreshold> CreditThresholds { get; set; }
    }
}