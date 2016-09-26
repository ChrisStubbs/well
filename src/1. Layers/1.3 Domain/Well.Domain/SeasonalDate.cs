namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;

    public class SeasonalDate : Entity<int>
    {
        public SeasonalDate()
        {
            this.Branches = new Collection<Branch>();
        }

        public string Description { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public Collection<Branch> Branches { get; set; }
    }
}