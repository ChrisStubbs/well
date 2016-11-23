namespace PH.Well.Services.EpodServices
{
    using PH.Well.Domain;
    using PH.Well.Services.Contracts;

    public class RouteMapper : IRouteMapper
    {
        public void Map(RouteHeader from, RouteHeader to)
        {
            to.RouteStatus = from.RouteStatus;
            to.RoutePerformanceStatusId = from.RoutePerformanceStatusId;
            to.AuthByPass = from.AuthByPass;
            to.NonAuthByPass = from.NonAuthByPass;
            to.ShortDeliveries = from.ShortDeliveries;
            to.DamagesRejected = from.DamagesRejected;
            to.DamagesAccepted = from.DamagesAccepted;
            to.NotRequired = from.NotRequired;
            to.Depot = from.Depot;
            to.StartDepotCode = from.StartDepotCode;
            to.ActualStopsCompleted = from.ActualStopsCompleted;
        }

        public void Map(Stop from, Stop to)
        {
            to.StopStatusCodeId = from.StopStatusCodeId;
            to.StopPerformanceStatusCodeId = from.StopPerformanceStatusCodeId;
            to.ByPassReasonId = from.ByPassReasonId;
        }

        public void Map(Job from, Job to)
        {
            to.ByPassReason = from.ByPassReason;
            to.PerformanceStatus = from.PerformanceStatus;
            to.InvoiceNumber = from.InvoiceNumber;
        }

        public void Map(JobDetail from, JobDetail to)
        {
            to.ShortQty = from.ShortQty;
            to.DeliveredQty = from.DeliveredQty;
        }
    }
}