namespace PH.Well.Domain
{
    using System;

    public class SeasonalDate : Entity<int>
    {
        public int BranchId { get; set; }

        public string Description { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}