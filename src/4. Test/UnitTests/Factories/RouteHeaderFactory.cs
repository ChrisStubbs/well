namespace PH.Well.UnitTests.Factories
{
    using System;
    using Well.Domain;

    public class RouteHeaderFactory : EntityFactory<RouteHeaderFactory, RouteHeader>
    {
        public RouteHeaderFactory()
        {
            this.Entity.Id = 1;
            this.Entity.RouteNumber = "202000";
            this.Entity.DriverName = "Chip Marklow";
            this.Entity.RouteStatusCode = "INPRO";
            this.Entity.RouteStatusDescription = "In Progress";
            this.Entity.CompanyId = 1;
            this.Entity.RouteDate = DateTime.Now;
            this.Entity.DriverName = "Alan Smith";
            this.Entity.StartDepotCode ="BIR";
            this.Entity.PlannedStops= 10;
            this.Entity.PlannedStops = 10;
            this.Entity.RoutesId = 1;
            this.Entity.PerformanceStatusCode = "";
            this.Entity.PerformanceStatusDescription = "Performance";
            this.Entity.LastRouteUpdate = DateTime.Now;
            this.Entity.AuthByPass = 1;
            this.Entity.NonAuthByPass = 1;
            this.Entity.ShortDeliveries = 0;
            this.Entity.DamagesRejected = 1;
            this.Entity.DamagesAccepted = 0;
            this.Entity.RouteOwnerId = 22;
        }
    }

}
