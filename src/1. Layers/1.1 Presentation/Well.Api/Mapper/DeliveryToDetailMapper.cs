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
                AccountName = detail.AccountName,
                AccountAddress = detail.AccountAddress,
                InvoiceNumber = detail.InvoiceNumber,
                ContactName = detail.ContactName,
                PhoneNumber = detail.PhoneNumber,
                MobileNumber = detail.MobileNumber,
                DeliveryType = detail.DeliveryType,
                IsException = detail.IsException,
                CanAction = detail.CanAction
            };

            foreach (var line in lines)
            {
                deliveryDetail.DeliveryLines.Add(new DeliveryLineModel
                {
                    JobId = line.JobId,
                    LineNo = line.LineNo,
                    ProductCode = line.ProductCode,
                    ProductDescription = line.ProductDescription,
                    Value = line.Value.ToString(),
                    InvoicedQuantity = line.InvoicedQuantity,
                    DeliveredQuantity = line.DeliveredQuantity,
                    DamagedQuantity = line.DamagedQuantity,
                    ShortQuantity = line.ShortQuantity,
                    Damages = line.Damages.Select(d => new DamageModel()
                    {
                        Quantity = d.Quantity,
                        ReasonCode = d.Reason.ToString()
                    }).ToList()
                });
            }

            return deliveryDetail;
        }
    }
}