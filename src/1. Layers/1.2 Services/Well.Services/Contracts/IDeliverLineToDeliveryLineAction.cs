namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain.ValueObjects;

    public interface IDeliverLineToDeliveryLineCredit
    {
        IEnumerable<DeliveryLineCredit> Map(IEnumerable<DeliveryLine> creditLines);
    }
}