namespace PH.Well.UnitTests.Factories
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Well.Domain.Enums;
    using Branch = Well.Domain.Branch;

    public class BranchFactory : EntityFactory<BranchFactory, Branch>
    {
        public BranchFactory()
        {
            this.Entity.Id = 22;
            this.Entity.Name = "Birtley";
        }

        public static List<Branch> GetAllBranches()
        {
            var branches = new List<Branch>
            {
                New.With(x => x.Name = "Medway").With(x => x.Id = 2).Build(),
                New.With(x => x.Name = "Coventry").With(x => x.Id = 3).Build(),
                New.With(x => x.Name = "Farham").With(x => x.Id = 5).Build(),
                New.With(x => x.Name = "Dunfermline").With(x => x.Id = 9).Build(),
                New.With(x => x.Name = "Leeds").With(x => x.Id = 14).Build(),
                New.With(x => x.Name = "Hemel").With(x => x.Id = 20).Build(),
                New.With(x => x.Name = "Birtley").With(x => x.Id = 22).Build(),
                New.With(x => x.Name = "Belfast").With(x => x.Id = 33).Build(),
                New.With(x => x.Name = "Brandon").With(x => x.Id = 42).Build(),
                New.With(x => x.Name = "Plymouth").With(x => x.Id = 55).Build(),
                New.With(x => x.Name = "Bristol").With(x => x.Id = 59).Build(),
                New.With(x => x.Name = "Haydock").With(x => x.Id = 82).Build(),
                New.With(x => x.Name = "Not Defined").With(x => x.Id = 99).Build()
            };

            return branches;
        }
    }
}
