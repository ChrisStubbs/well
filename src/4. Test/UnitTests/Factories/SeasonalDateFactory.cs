namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.ObjectModel;

    using Well.Domain;
    
    public class SeasonalDateFactory : EntityFactory<SeasonalDateFactory, SeasonalDate>
    {
        public SeasonalDateFactory()
        {
            this.Entity.Id = 1;
            this.Entity.Description = "Halloween";
            this.Entity.From = DateTime.Now.AddDays(-10);
            this.Entity.To = DateTime.Now.AddDays(10);

            this.Entity.Branches = new Collection<Branch>();
        }

        public SeasonalDateFactory WithBranch(Branch branch)
        {
            this.Entity.Branches.Add(branch);

            return this;
        }
    }
}
