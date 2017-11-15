namespace PH.Well.Services.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    public class DeliveryLineCreditMapper : IDeliveryLineCreditMapper
    {

        public List<DeliveryLineCredit> Map(Job job)
        {
            var credits = new List<DeliveryLineCredit>();
            foreach (var lineItem in job.LineItems)
            {
                var liCredits = lineItem.LineItemActions.Where(x => x.DeliveryAction == DeliveryAction.Credit).Select(x =>
                    new DeliveryLineCredit
                    {
                        JobId = job.Id,
                        Reason = (int) x.Reason,
                        Source = (int) x.Source,
                        Quantity = x.Quantity,
                        ProductCode = lineItem.ProductCode
                    }
                );
                credits.AddRange(liCredits);
            }
            return credits;
        }

        public List<DeliveryLineUplift> MapUplift(Job job)
        {
            var uplifts = new List<DeliveryLineUplift>();
            foreach (var lineItem in job.LineItems)
            {
                var liUplifts = lineItem.LineItemActions.Where(x => x.DeliveryAction == DeliveryAction.Credit || x.DeliveryAction == DeliveryAction.Close).Select(x =>
                    new DeliveryLineUplift
                    {
                        JobId = job.Id,
                        Reason = (int)x.Reason,
                        Source = (int)x.Source,
                        Quantity = x.Quantity,
                        ProductCode = lineItem.ProductCode,
                        Action = x.DeliveryAction
                    }
                );
                uplifts.AddRange(liUplifts);
            }
            return uplifts;
        }
    }
}