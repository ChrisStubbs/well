namespace PH.Well.Services.EpodServices
{
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.Extensions;
    using Repositories.Contracts;

    public class AdamImportMapper : IAdamImportMapper
    {
        public void MapStop(Stop source, Stop destination)
        {
            destination.PlannedStopNumber = source.PlannedStopNumber;
            destination.RouteHeaderId = source.RouteHeaderId;
            destination.RouteHeaderCode = source.RouteHeaderCode;
            destination.DropId = source.DropId;
            destination.DeliveryDate = source.DeliveryDate;

            destination.AllowOvers = source.AllowOvers;
            destination.CustUnatt = source.CustUnatt;
            destination.PHUnatt = source.PHUnatt;
            destination.AccountBalance = source.AccountBalance;
        }

        public RouteHeaderFromImportedFile MapRouteHeader(RouteHeader source)
        {
            return new RouteHeaderFromImportedFile
            {
                Id = source.Id,
                PlannedStops = source.PlannedStops,
                RouteDate = source.RouteDate,
                RouteNumber = source.RouteNumber,
                RouteOwnerId = source.RouteOwnerId,
                StartDepot = source.StartDepot
            };
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
            destination.UpliftAction = source.UpliftAction;
            destination.IsSubOuterQuantity = source.IsSubOuterQuantity;
        }
    }
}