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
            this.Entity.CompanyID = 1;
            this.Entity.RouteDate = DateTime.Now;
            this.Entity.DriverName = "Alan Smith";
            this.Entity.VehicleReg= "EYO GR9";
            this.Entity.StartDepotCode ="001";
            this.Entity.PlannedRouteStartTime = "09:00";
            this.Entity.PlannedRouteFinishTime= "11:00";
            this.Entity.PlannedDistance = 20.5m;
            this.Entity.PlannedTravelTime= "02:00";
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
        }
    }

}
