namespace PH.Well.Services.EpodServices
{
    using Contracts;
    using Domain;
    using Domain.Enums;

    public class AdamImportMapper : IAdamImportMapper
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

        public void MapJob(Job source, Job destination)
        {
            destination.Sequence = source.Sequence;
            destination.JobTypeCode = source.JobTypeCode;
            destination.PhAccount = source.PhAccount;
            destination.PickListRef = source.PickListRef;
            destination.InvoiceNumber = source.InvoiceNumber;
            destination.CustomerRef = source.CustomerRef;
            destination.PerformanceStatus = PerformanceStatus.Notdef;
            destination.Picked = source.Picked;
            destination.OrdOuters = source.OrdOuters;
            destination.InvOuters = source.InvOuters;
            destination.AllowSoCrd = source.AllowSoCrd;
            destination.Cod = source.Cod;
            destination.AllowReOrd = source.AllowReOrd;
            destination.TotalOutersShort = source.TotalOutersShort;
        }

        public void MapJobDetail(JobDetail source, JobDetail destination)
        {
            destination.LineNumber = source.LineNumber;
            destination.PhProductCode = source.PhProductCode;
            destination.ProdDesc = source.ProdDesc;
            destination.OrderedQty = source.OrderedQty;
            destination.UnitMeasure = source.UnitMeasure;
            destination.PhProductType = source.PhProductType;
            destination.PackSize = source.PackSize;
            destination.SingleOrOuter = source.SingleOrOuter;
            destination.SSCCBarcode = source.SSCCBarcode; //tobacco bag barcode
            destination.SkuGoodsValue = source.SkuGoodsValue;
            destination.OriginalDespatchQty = source.OriginalDespatchQty;
            destination.NetPrice = source.NetPrice;
        }
    }
}