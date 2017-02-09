namespace PH.Well.Services
{
    using System.Collections.Generic;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services.Contracts;

    public class DeliverLineToDeliveryLineCreditMapper : IDeliverLineToDeliveryLineCreditMapper
    {
        public List<DeliveryLineCredit> Map(IEnumerable<DeliveryLine> creditLines)
        {
            var credits = new List<DeliveryLineCredit>();

            foreach (var line in creditLines)
            {
                if (line.ShortQuantity > 0)
                {
                    credits.Add(new DeliveryLineCredit
                    {
                        JobId = line.JobId,
                        Reason = line.JobDetailReasonId,
                        Source = line.JobDetailSourceId,
                        Quantity = line.ShortQuantity,
                        ProductCode = line.ProductCode
                    });
                }
                
                foreach (var damage in line.Damages)
                {
                    if ((DeliveryAction)damage.DamageActionId == DeliveryAction.Credit)
                    {
                        credits.Add(new DeliveryLineCredit
                        {
                            JobId = line.JobId,
                            Reason = damage.JobDetailReasonId,
                            Source = damage.JobDetailSourceId,
                            Quantity = damage.Quantity,
                            ProductCode = line.ProductCode
                        });
                    }
                }
            }

            return credits;
        }
    }
}
