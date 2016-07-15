namespace PH.Well.UnitTests.Factories
{
    using Well.Domain;
    using Well.Domain.Enums;

    public class RouteHeaderFactory : EntityFactory<RouteHeaderFactory,RouteHeader>
    {
        public RouteHeaderFactory()
        {
            this.Entity.Id = 1;
            this.Entity.RouteNumber = "2";
            this.Entity.DriverName = "Chip Marklow";
            this.Entity.RouteStatus = RouteStatusCode.Inpro;
        }
    }

}
