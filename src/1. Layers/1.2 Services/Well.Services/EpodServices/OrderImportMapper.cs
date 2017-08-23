namespace PH.Well.Services.EpodServices
{
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services.Contracts;

    public class OrderImportMapper : IOrderImportMapper
    {
        public void Map(StopUpdate from, Stop to)
        {
            if (!string.IsNullOrEmpty(from.PlannedStopNumber))
            {
                to.PlannedStopNumber = from.PlannedStopNumber;
            }
            to.ShellActionIndicator = from.ShellActionIndicator;
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
            to.Picked = from.Picked;
            to.OrdOuters = from.OrdOuters;
            to.InvOuters = from.InvOuters;
            to.AllowSoCrd = from.AllowSoCrd;
            to.Cod = from.Cod;
            to.AllowReOrd = from.AllowReOrd;
            to.TotalOutersShort = from.TotalOutersShort;
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
            to.SSCCBarcode = from.SSCCBarcode; //tobacco bag barcode
            to.SkuGoodsValue = from.SkuGoodsValue;
            to.OriginalDespatchQty = from.OriginalDespatchQty;
            to.NetPrice = from.NetPrice;
        }

    }
}