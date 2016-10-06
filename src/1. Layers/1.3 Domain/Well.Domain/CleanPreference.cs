namespace PH.Well.Domain
{
    using System.Collections.ObjectModel;

    public class CleanPreference : Entity<int>
    {
        public CleanPreference()
        {
            this.Branches = new Collection<Branch>();
        }

        public int Days { get; set; }

        public Collection<Branch> Branches { get; set; }
    }
}