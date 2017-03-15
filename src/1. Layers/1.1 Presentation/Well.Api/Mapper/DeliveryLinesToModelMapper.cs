namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Common.Extensions;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    public class DeliveryLinesToModelMapper : IDeliveryLinesToModelMapper
    {
        public DeliveryActionModel Map(IEnumerable<DeliveryLine> deliveryLines)
        {
            var model = new DeliveryActionModel();
            model.JobId = deliveryLines.First().JobId;
            model.TotalCreditThreshold = deliveryLines.Sum(x => x.CreditValueForThreshold());

            foreach (var line in deliveryLines)
            {
                if (line.ShortQuantity == 0 && !line.Damages.Any()) continue;

                var deliveryLineModel = new DeliveryLineModel
                {
                    JobId = line.JobId,
                    JobDetailId = line.JobDetailId,
                    ProductCode = line.ProductCode,
                    ProductDescription = line.ProductDescription,
                    ShortQuantity = line.ShortQuantity,
                    ShortsAction = Enum<DeliveryAction>.GetDescription((DeliveryAction)line.ShortsActionId),
                    JobDetailReason = Enum<JobDetailReason>.GetDescription((JobDetailReason)line.JobDetailReasonId),
                    JobDetailSource = Enum<JobDetailSource>.GetDescription((JobDetailSource)line.JobDetailSourceId)
                };

                foreach (var damage in line.Damages)
                {
                    var damageModel = new DamageModel
                    {
                        Quantity = damage.Quantity,
                        DamageAction = Enum<DeliveryAction>.GetDescription((DeliveryAction)damage.DamageActionId),
                        JobDetailReason =
                            Enum<JobDetailReason>.GetDescription((JobDetailReason)damage.JobDetailReasonId),
                        JobDetailSource =
                            Enum<JobDetailSource>.GetDescription((JobDetailSource)damage.JobDetailSourceId)
                    };

                    deliveryLineModel.Damages.Add(damageModel);
                }

                model.Lines.Add(deliveryLineModel);
            }

            return model;
        }
    }
}