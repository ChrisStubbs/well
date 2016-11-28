namespace PH.Well.Services.EpodServices
{
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
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

        public void Map(StopUpdate from, Stop to)
        {
            to.PlannedStopNumber = from.PlannedStopNumber;
            to.ShellActionIndicator = from.ShellActionIndicator;
            to.StopStatusCodeId = (int)StopStatus.Notdef;
            to.StopPerformanceStatusCodeId = (int)PerformanceStatus.Notdef;
            to.ByPassReasonId = (int)ByPassReasons.Notdef;
        }

        public void Map(Job from, Job to)
        {
            to.ByPassReason = from.ByPassReason;
            to.PerformanceStatus = from.PerformanceStatus;
            to.InvoiceNumber = from.InvoiceNumber;
        }

        public void Map(JobUpdate from, Job to)
        {
            to.Sequence = from.Sequence;
            to.JobTypeCode = from.JobTypeCode;
            to.PhAccount = from.PhAccount;
            to.PickListRef = from.PickListRef;
            to.InvoiceNumber = from.InvoiceNumber;
            to.CustomerRef = from.CustomerRef;
            to.PerformanceStatus = PerformanceStatus.Notdef;
            to.ByPassReason = ByPassReasons.Notdef;
        }

        public void Map(JobDetail from, JobDetail to)
        {
            to.ShortQty = from.ShortQty;
            to.DeliveredQty = from.DeliveredQty;
        }

        public void Map(JobDetailUpdate from, JobDetail to)
        {
            to.LineNumber = from.LineNumber;
            to.PhProductCode = from.PhProductCode;
            to.ProdDesc = from.ProdDesc;
            to.OrderedQty = from.OrderedQty;
            to.UnitMeasure = from.UnitMeasure;
            to.PhProductType = from.PhProductType;
            to.PackSize = from.PackSize;
            to.SingleOrOuter = from.SingleOrOuter;
            to.SsccBarcode = from.SsccBarcode;
            to.SkuGoodsValue = from.SkuGoodsValue;
        }
    }
}