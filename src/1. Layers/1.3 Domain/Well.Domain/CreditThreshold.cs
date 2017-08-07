namespace PH.Well.Domain
{
    using System.Collections.ObjectModel;

    using Enums;

    public class CreditThreshold : Entity<int>
    {
        public ThresholdLevel Level { get; set; }

        public decimal Threshold { get; set; }

        public string Description { get; set; }
    }
}