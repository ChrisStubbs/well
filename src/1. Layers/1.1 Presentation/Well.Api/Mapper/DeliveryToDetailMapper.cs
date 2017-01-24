namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Domain.ValueObjects;

    public class DeliveryToDetailMapper : IDeliveryToDetailMapper
    {
        public DeliveryDetailModel Map(IEnumerable<DeliveryLine> lines, DeliveryDetail detail)
        {
            var deliveryDetail = new DeliveryDetailModel
            {
                Id = detail.Id,
                AccountCode = detail.AccountCode,
                OuterCount = detail.OuterCount,
                OuterDiscrepancyFound = detail.OuterDiscrepancyFound,
                TotalOutersShort = detail.TotalOutersShort,
                AccountName = detail.AccountName,
                AccountAddress = detail.AccountAddress,
                InvoiceNumber = detail.InvoiceNumber,
                ContactName = detail.ContactName,
                PhoneNumber = detail.PhoneNumber,
                MobileNumber = detail.MobileNumber,
                DeliveryType = detail.DeliveryType,
                IsException = detail.IsException,
                CanAction = detail.CanAction,
                GrnNumber = detail.GrnNumber,
                BranchId = detail.BranchId
            };

            foreach (DeliveryLine line in lines.Where(x => x.IsClean))
            {
                deliveryDetail.CleanDeliveryLines.Add(new DeliveryLineModel
                {
                    JobDetailId = line.JobDetailId,
                    JobId = line.JobId,
                    LineNo = line.LineNo,
                    ProductCode = line.ProductCode,
                    ProductDescription = line.ProductDescription,
                    Value = line.Value.ToString(),
                    InvoicedQuantity = line.InvoicedQuantity,
                    DeliveredQuantity = line.DeliveredQuantity,
                    DamagedQuantity = line.DamagedQuantity,
                    ShortQuantity = line.ShortQuantity,
                    LineDeliveryStatus = line.LineDeliveryStatus,
                    JobDetailReasonId = line.JobDetailReasonId,
                    JobDetailSourceId = line.JobDetailSourceId,
                    ShortsActionId = line.ShortsActionId,
                    Damages = line.Damages.Select(d => new DamageModel()
                    {
                        Quantity = d.Quantity,
                        JobDetailReasonId = d.JobDetailReasonId,
                        JobDetailSourceId = d.JobDetailSourceId,
                        DamageActionId = d.DamageActionId
                    }).ToList()
                });
            }

            foreach (DeliveryLine line in lines.Where(x => !x.IsClean))
            {
                deliveryDetail.ExceptionDeliveryLines.Add(new DeliveryLineModel
                {
                    JobDetailId = line.JobDetailId,
                    JobId = line.JobId,
                    LineNo = line.LineNo,
                    ProductCode = line.ProductCode,
                    ProductDescription = line.ProductDescription,
                    Value = line.Value.ToString(),
                    InvoicedQuantity = line.InvoicedQuantity,
                    DeliveredQuantity = line.DeliveredQuantity,
                    DamagedQuantity = line.DamagedQuantity,
                    ShortQuantity = line.ShortQuantity,
                    LineDeliveryStatus = line.LineDeliveryStatus,
                    JobDetailReasonId = line.JobDetailReasonId,
                    JobDetailSourceId = line.JobDetailSourceId,
                    ShortsActionId = line.ShortsActionId,
                    Damages = line.Damages.Select(d => new DamageModel()
                    {
                        Quantity = d.Quantity,
                        JobDetailReasonId = d.JobDetailReasonId,
                        JobDetailSourceId = d.JobDetailSourceId,
                        DamageActionId = d.DamageActionId 
                    }).ToList()
                });
            }

            return deliveryDetail;
        }
    }
}