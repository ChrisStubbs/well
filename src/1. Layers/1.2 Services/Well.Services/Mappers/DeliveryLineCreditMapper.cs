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
        public List<DeliveryLineCredit> Map(IEnumerable<LineItemActionSubmitModel> lineItems)
        {
            return lineItems.Select(line => new DeliveryLineCredit
            {
                JobId = line.JobId,
                Reason = (int) line.Reason,
                Source = (int) line.Source,
                Quantity = line.Quantity,
                ProductCode = line.ProductCode
            }).ToList();
        }

        public List<DeliveryLineCredit> Map(Job job)
        {
            throw new NotImplementedException("TODO: Write test on code below");
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
    }
}