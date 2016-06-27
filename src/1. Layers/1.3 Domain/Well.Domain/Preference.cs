namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;

    public class Preference : Entity<int>
    {
        public Preference()
        {
            this.Branches = new Collection<Branch>();
        }

        public TimeSpan CleanDeliveryHeldFor { get; set; }

        public TimeSpan DirtyDeliveryHeldFor { get; set; }

        public Collection<Branch> Branches { get; set; }
    }
}