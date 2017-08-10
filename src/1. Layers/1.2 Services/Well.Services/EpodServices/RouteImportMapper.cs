namespace PH.Well.Services.EpodServices
{
    using Contracts;
    using Domain;

    public class RouteImportMapper : IRouteImportMapper
    {
        public void MapStop(Stop from, Stop to)
        {
            to.PlannedStopNumber = from.PlannedStopNumber;
            to.RouteHeaderId = from.RouteHeaderId;
            to.RouteHeaderCode = from.RouteHeaderCode;
            to.DropId = from.DropId;
            to.DeliveryDate = from.DeliveryDate;

            to.AllowOvers = from.AllowOvers;
            to.CustUnatt = from.CustUnatt;
            to.PHUnatt = from.PHUnatt;
            to.AccountBalance = from.AccountBalance;
        }

        public RouteHeader MapRouteHeader(RouteHeader source, RouteHeader destination)
        {
            destination.StartDepotCode = source.StartDepotCode;
            destination.RouteDate = source.RouteDate;
            destination.RouteNumber = source.RouteNumber;
            destination.PlannedStops = source.PlannedStops;
            destination.RouteOwnerId = source.RouteOwnerId;
            return destination;
        }
    }
}