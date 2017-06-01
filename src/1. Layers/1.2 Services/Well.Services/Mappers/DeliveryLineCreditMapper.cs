namespace PH.Well.Services.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
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
    }
}