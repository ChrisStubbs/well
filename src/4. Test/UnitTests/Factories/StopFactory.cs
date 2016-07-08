namespace PH.Well.UnitTests.Factories
{
    using Domain;

    public class StopFactory : EntityFactory<StopFactory,Stop>
    {
        public StopFactory()
        {
            this.Entity.Id = 1;
        }
    }
}
