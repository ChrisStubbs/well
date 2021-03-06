﻿namespace PH.Well.Domain
{
    public class User : Entity<int>
    {
        public string Name { get; set; }

        public string IdentityName { get; set; }

        public string FriendlyName => this.Name.Replace(' ', '-');

        public string JobDescription { get; set; }

        public string Domain { get; set; }

        public int? CreditThresholdId { get; set; }

        public CreditThreshold CreditThreshold { get; set; }

        public decimal? Threshold => CreditThreshold?.Threshold;
    }
}