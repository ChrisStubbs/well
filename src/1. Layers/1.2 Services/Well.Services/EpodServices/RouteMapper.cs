namespace PH.Well.Services.EpodServices
{
    using System.Linq;

    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services.Contracts;

    public class RouteMapper : IRouteMapper
    {
        public void Map(RouteHeader from, RouteHeader to)
        {
            to.RouteStatusCode = from.RouteStatusCode;
            to.RouteStatusDescription = from.RouteStatusDescription;
            to.PerformanceStatusCode = from.PerformanceStatusCode;
            to.PerformanceStatusDescription = from.PerformanceStatusDescription;
            to.AuthByPass = from.AuthByPass;
            to.NonAuthByPass = from.NonAuthByPass;
            to.ShortDeliveries = from.ShortDeliveries;
            to.DamagesRejected = from.DamagesRejected;
            to.DamagesAccepted = from.DamagesAccepted;
            to.StartDepotCode = from.StartDepotCode;
            to.ActualStopsCompleted = from.ActualStopsCompleted;
        }

        public void Map(Stop from, Stop to)
        {
            to.StopStatusCode = from.StopStatusCode;
            to.StopStatusDescription = from.StopStatusDescription;
            to.PerformanceStatusCode = from.PerformanceStatusCode;
            to.PerformanceStatusDescription = from.PerformanceStatusDescription;
            to.StopByPassReason = from.StopByPassReason;
        }

        public void Map(StopUpdate from, Stop to)
        {
            to.PlannedStopNumber = from.PlannedStopNumber;
            to.ShellActionIndicator = from.ShellActionIndicator;
        }

        public void Map(Job from, Job to)
        {
            to.JobByPassReason = from.JobByPassReason;
            to.PerformanceStatus = from.PerformanceStatus;
            to.InvoiceNumber = from.InvoiceNumber;
            to.JobDetails = from.JobDetails;
            to.GrnNumberUpdate = from.GrnNumber;
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

            // TODO refactor to check that we have these attributes else will throw an error
            to.EntityAttributes.Add(from.EntityAttributes.FirstOrDefault(x => x.Code == "PICKED"));
            to.EntityAttributes.Add(from.EntityAttributes.FirstOrDefault(x => x.Code == "ORDOUTERS"));
            to.EntityAttributes.Add(from.EntityAttributes.FirstOrDefault(x => x.Code == "INVOUTERS"));
            to.EntityAttributes.Add(from.EntityAttributes.FirstOrDefault(x => x.Code == "ALLOWSOCRD"));
            to.EntityAttributes.Add(from.EntityAttributes.FirstOrDefault(x => x.Code == "COD"));
            to.EntityAttributes.Add(from.EntityAttributes.FirstOrDefault(x => x.Code == "ALLOWREORD"));
        }

        public void Map(JobDetail from, JobDetail to)
        {
            to.ShortQty = from.ShortQty;
            to.DeliveredQty = from.DeliveredQty;
            to.OriginalDespatchQty = from.OriginalDespatchQty;
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
            to.OriginalDespatchQty = from.OriginalDespatchQty;
        }
    }
}