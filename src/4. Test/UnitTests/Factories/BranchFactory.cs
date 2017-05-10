namespace PH.Well.UnitTests.Factories
{
    using Well.Domain.Enums;
    using Branch = Well.Domain.Branch;

    public class BranchFactory : EntityFactory<BranchFactory, Branch>
    {
        public BranchFactory()
        {
            this.Entity.Id = 22;
            this.Entity.Name = "Birtley";
        }
    }
}
