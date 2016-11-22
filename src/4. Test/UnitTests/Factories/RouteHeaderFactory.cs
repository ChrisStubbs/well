namespace PH.Well.UnitTests.Factories
{
    using System;
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
            this.Entity.RouteStatusCode = RouteStatusCode.Compl.ToString();
            this.Entity.CompanyId = 1;
            this.Entity.RouteDate = DateTime.Now;
            this.Entity.DriverName = "Alan Smith";
            this.Entity.StartDepotCode ="BIR";
            this.Entity.PlannedStops= 10;
            this.Entity.PlannedStops = 10;
            this.Entity.RoutesId = 1;
            this.Entity.RoutePerformanceStatusId = 6;
            this.Entity.LastRouteUpdate = DateTime.Now;
            this.Entity.AuthByPass = 1;
            this.Entity.NonAuthByPass = 1;
            this.Entity.ShortDeliveries = 0;
            this.Entity.DamagesRejected = 1;
            this.Entity.DamagesAccepted = 0;
            this.Entity.NotRequired = 0;
            this.Entity.Depot = "001";
            //this.Entity.RouteOwner = "BIR";
            this.Entity.RouteOwnerId = 22;
        }
    }

}
