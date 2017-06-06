namespace PH.Well.Services.Mappers
{
    using System.Collections.Generic;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    public class JobDetailToDeliveryLineCreditMapper : IJobDetailToDeliveryLineCreditMapper
    {
        public List<DeliveryLineCredit> Map(IEnumerable<JobDetail> creditLines)
        {
            var credits = new List<DeliveryLineCredit>();

            foreach (var line in creditLines)
            {
                if (line.ShortQty > 0)
                {
                    credits.Add(new DeliveryLineCredit
                    {
                        JobId = line.JobId,
                        Reason = line.JobDetailReasonId,
                        Source = line.JobDetailSourceId,
                        Quantity = line.ShortQty,
                        ProductCode = line.PhProductCode
                    });
                }
                
                foreach (var damage in line.JobDetailDamages)
                {
                    if ((DeliveryAction)damage.DamageActionId == DeliveryAction.Credit)
                    {
                        credits.Add(new DeliveryLineCredit
                        {
                            JobId = line.JobId,
                            Reason = damage.JobDetailReasonId,
                            Source = damage.JobDetailSourceId,
                            Quantity = damage.Qty,
                            ProductCode = line.PhProductCode
                        });
                    }
                }
            }

            return credits;
        }
    }
}
