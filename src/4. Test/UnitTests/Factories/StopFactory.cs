namespace PH.Well.UnitTests.Factories
{
    using Well.Domain;
    
    public class StopFactory : EntityFactory<StopFactory,Stop>
    {
        public StopFactory()
        {
            this.Entity.Id = 1;
        }
    }
}
