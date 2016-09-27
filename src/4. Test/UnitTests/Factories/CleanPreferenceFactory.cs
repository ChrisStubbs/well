namespace PH.Well.UnitTests.Factories
{
    using System.Collections.ObjectModel;

    using Well.Domain;
    
    public class CleanPreferenceFactory : EntityFactory<CleanPreferenceFactory, CleanPreference>
    {
        public CleanPreferenceFactory()
        {
            this.Entity.Id = 1;
            this.Entity.Days = 1;

            this.Entity.Branches = new Collection<Branch>();
        }

        public CleanPreferenceFactory WithBranch(Branch branch)
        {
            this.Entity.Branches.Add(branch);

            return this;
        }
    }
}
