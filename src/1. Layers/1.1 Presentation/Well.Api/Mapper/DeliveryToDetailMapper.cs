namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Enums;
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
                CashOnDelivery = detail.CashOnDelivery,
                InvoiceNumber = detail.InvoiceNumber,
                ContactName = detail.ContactName,
                PhoneNumber = detail.PhoneNumber,
                MobileNumber = detail.MobileNumber,
                JobStatus = detail.JobStatus,
                CanAction = detail.CanAction,
                IsPendingCredit = detail.IsPendingCredit,
                GrnNumber = detail.GrnNumber,
                BranchId = detail.BranchId,
                GrnProcessType = detail.GrnProcessType,
                ProofOfDelivery = detail.ProofOfDelivery,
                IsProofOfDelivery = detail.IsProofOfDelivery,
                DetailOutersShort = detail.DetailOutersShort,
                ToBeAdvisedCount = detail.ToBeAdvisedCount
            };

            foreach (var line in lines)
            {
                var newItem = new DeliveryLineModel
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
                    IsHighValue = line.IsHighValue,
                    Damages = line.Damages.Select(d => new DamageModel()
                    {
                        Quantity = d.Quantity,
                        JobDetailReasonId = d.JobDetailReasonId,
                        JobDetailSourceId = d.JobDetailSourceId,
                        DamageActionId = d.DamageActionId
                    }).ToList()
                };

                if (deliveryDetail.JobStatus == JobStatus.Clean.ToString() || line.IsClean)
                {
                    deliveryDetail.CleanDeliveryLines.Add(newItem);
                }
                else
                {
                    deliveryDetail.ExceptionDeliveryLines.Add(newItem);
                }
            }

            deliveryDetail.CleanDeliveryLines = deliveryDetail.CleanDeliveryLines.OrderBy(p => p.LineNo).ToList();
            deliveryDetail.ExceptionDeliveryLines = deliveryDetail.ExceptionDeliveryLines.OrderBy(p => p.LineNo).ToList();

            return deliveryDetail;
        }
    }
}