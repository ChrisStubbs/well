namespace PH.Well.Services.EpodServices
{
    using Contracts;
    using Domain;

    public class EpodImportMapper : IEpodImportMapper
    {
        public void MapStop(Stop from, Stop to)
        {
            to.StopStatusCode = from.StopStatusCode;
            to.StopStatusDescription = from.StopStatusDescription;
            to.PerformanceStatusCode = from.PerformanceStatusCode;
            to.PerformanceStatusDescription = from.PerformanceStatusDescription;
            to.StopByPassReason = from.StopByPassReason;
        }

        public void MapJob(Job source, Job destination)
        {
            destination.JobByPassReason = source.JobByPassReason;
            destination.PerformanceStatus = source.PerformanceStatus;
            destination.InvoiceNumber = source.InvoiceNumber;
            destination.GrnNumber = source.GrnNumber;
            destination.OuterCount = source.OuterCount;
            // destination.OuterDiscrepancyUpdate = source.OuterDiscrepancyFound;
            destination.TotalOutersOverUpdate = source.TotalOutersOver;
            destination.TotalOutersShort = source.TotalOutersShort;
            destination.DetailOutersOverUpdate = source.DetailOutersOver;
            destination.DetailOutersShortUpdate = source.DetailOutersShort;
           
        }

        public void MergeRouteHeader(RouteHeader fileRouteHeader, RouteHeader dbRouteHeader)
        {
            dbRouteHeader.RouteStatusCode = fileRouteHeader.RouteStatusCode;
            dbRouteHeader.RouteStatusDescription = fileRouteHeader.RouteStatusDescription;
            dbRouteHeader.PerformanceStatusCode = fileRouteHeader.PerformanceStatusCode;
            dbRouteHeader.PerformanceStatusDescription = fileRouteHeader.PerformanceStatusDescription;
            dbRouteHeader.AuthByPass = fileRouteHeader.AuthByPass;
            dbRouteHeader.NonAuthByPass = fileRouteHeader.NonAuthByPass;
            dbRouteHeader.ShortDeliveries = fileRouteHeader.ShortDeliveries;
            dbRouteHeader.DamagesRejected = fileRouteHeader.DamagesRejected;
            dbRouteHeader.DamagesAccepted = fileRouteHeader.DamagesAccepted;
            dbRouteHeader.ActualStopsCompleted = fileRouteHeader.ActualStopsCompleted;
            dbRouteHeader.DriverName = fileRouteHeader.DriverName;

            fileRouteHeader.Id = dbRouteHeader.Id;
            fileRouteHeader.RouteOwnerId = dbRouteHeader.RouteOwnerId;
            fileRouteHeader.RouteNumber = dbRouteHeader.RouteNumber;
        }

        public void MapJobDetail(JobDetail source, JobDetail destination)
        {
            destination.ShortQty = source.ShortQty;
            destination.DeliveredQty = source.DeliveredQty;
        }
    }
    
}