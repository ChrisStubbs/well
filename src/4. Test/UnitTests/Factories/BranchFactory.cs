namespace PH.Well.UnitTests.Factories
{
    using Well.Domain;
    
    public class BranchFactory : EntityFactory<BranchFactory, Branch>
    {
        public BranchFactory()
        {
            this.Entity.Id = 1;
            this.Entity.Name = "Haydock";
        }
    }
}
