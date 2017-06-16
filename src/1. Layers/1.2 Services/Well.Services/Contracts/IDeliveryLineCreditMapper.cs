namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface IDeliveryLineCreditMapper
    {
        List<DeliveryLineCredit> Map(IEnumerable<LineItemActionSubmitModel> creditLines);
        List<DeliveryLineCredit> Map(Job job);
    }
}