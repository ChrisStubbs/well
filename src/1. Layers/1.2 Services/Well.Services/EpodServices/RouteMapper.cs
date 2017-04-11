namespace PH.Well.Services.EpodServices
{
    using System.Collections.Generic;
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
           // to.StartDepotCode = from.StartDepotCode;
            to.ActualStopsCompleted = from.ActualStopsCompleted;
            to.DriverName = from.DriverName;
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
            to.OuterCountUpdate = from.OuterCount;
            to.OuterDiscrepancyUpdate = from.OuterDiscrepancyFound;
            to.TotalOutersOverUpdate = from.TotalOutersOver;
            to.TotalOutersShortUpdate = from.TotalOutersShort;
            to.DetailOutersOverUpdate = from.DetailOutersOver;
            to.DetailOutersShortUpdate = from.DetailOutersShort;
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

            this.AddAttribute(to.EntityAttributes, from.EntityAttributes, "PICKED");
            this.AddAttribute(to.EntityAttributes, from.EntityAttributes, "ORDOUTERS");
            this.AddAttribute(to.EntityAttributes, from.EntityAttributes, "INVOUTERS");
            this.AddAttribute(to.EntityAttributes, from.EntityAttributes, "ALLOWSOCRD");
            this.AddAttribute(to.EntityAttributes, from.EntityAttributes, "COD");
            this.AddAttribute(to.EntityAttributes, from.EntityAttributes, "ALLOWREORD");

        }

        public void Map(JobDetail from, JobDetail to)
        {
            to.ShortQty = from.ShortQty;
            to.DeliveredQty = from.DeliveredQty;
            to.OriginalDespatchQty = from.OriginalDespatchQty;

            this.AddAttributeValues(to.EntityAttributeValues, from.EntityAttributeValues, "LINESTATUS");
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

        private void AddAttribute(List<EntityAttribute> jobAttributes, List<EntityAttribute> jobUpdateAttributes,
            string name)
        {
            var attribute = jobUpdateAttributes.FirstOrDefault(x => x.Code == name);

            if (attribute != null)
            {
                jobAttributes.Add(attribute);
            }
        }

        private void AddAttributeValues(List<EntityAttributeValue> jobAttributes, List<EntityAttributeValue> jobUpdateAttributes,
          string name)
        {
            var attribute = jobUpdateAttributes.FirstOrDefault(x => x.EntityAttribute.Code == name);

            if (attribute != null)
            {
                jobAttributes.Add(attribute);
            }
        }
    }
}