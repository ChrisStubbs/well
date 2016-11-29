namespace PH.Well.UnitTests.Factories
{
    using System.Collections.Generic;
    using Well.Domain;

    public class RouteDeliveryFactory : EntityFactory<RouteDeliveryFactory, RouteDelivery>
    {
        public RouteDeliveryFactory()
        {
            this.Entity.RouteId = 90;
            this.Entity.RouteHeaders = new List<RouteHeader>();
        }
    }
}
