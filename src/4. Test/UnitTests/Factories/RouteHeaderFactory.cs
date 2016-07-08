namespace PH.Well.UnitTests.Factories
{
    using Domain;

    public class RouteHeaderFactory : EntityFactory<RouteHeaderFactory,RouteHeader>
    {
        public RouteHeaderFactory()
        {
            this.Entity.Id = 1;
        }
    }

}
