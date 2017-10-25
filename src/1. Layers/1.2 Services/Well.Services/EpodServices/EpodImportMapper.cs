namespace PH.Well.Services.EpodServices
{
    using Contracts;
    using Domain;
    using Domain.Extensions;

    public class EpodImportMapper : IEpodImportMapper
    {
        public void MapStop(Stop source, Stop destination)
        {
            destination.StopStatusCode = source.StopStatusCode;
            destination.StopStatusDescription = source.StopStatusDescription;
            destination.PerformanceStatusCode = source.PerformanceStatusCode;
            destination.PerformanceStatusDescription = source.PerformanceStatusDescription;
            destination.StopByPassReason = source.StopByPassReason;
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
            destination.Picked = source.Picked;
            destination.AllowSoCrd = source.AllowSoCrd;
            destination.JobByPassReason = source.JobByPassReason;
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
            dbRouteHeader.DriverName = fileRouteHeader.AgencyDriverName ?? fileRouteHeader.DriverName;

            fileRouteHeader.Id = dbRouteHeader.Id;
            fileRouteHeader.RouteOwnerId = dbRouteHeader.RouteOwnerId;
            fileRouteHeader.RouteNumber = dbRouteHeader.RouteNumber;
        }

        public void MapJobDetail(JobDetail source, JobDetail destination)
        {
            destination.ShortQty = source.ShortQty;
            destination.DeliveredQty = source.DeliveredQty;
            destination.LineDeliveryStatus = source.LineDeliveryStatus;
        }
    }
    
}