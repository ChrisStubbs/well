namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain.ValueObjects;

    public interface IDeliverLineToDeliveryLineCreditMapper
    {
        List<DeliveryLineCredit> Map(IEnumerable<DeliveryLine> creditLines);
    }
}