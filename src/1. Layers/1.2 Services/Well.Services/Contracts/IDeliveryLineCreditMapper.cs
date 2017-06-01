namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IDeliveryLineCreditMapper
    {
        List<DeliveryLineCredit> Map(IEnumerable<LineItemActionSubmitModel> creditLines);
    }
}