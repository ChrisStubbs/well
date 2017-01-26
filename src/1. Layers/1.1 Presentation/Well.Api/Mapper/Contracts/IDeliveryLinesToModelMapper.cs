namespace PH.Well.Api.Mapper.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Api.Models;
    using PH.Well.Domain.ValueObjects;

    public interface IDeliveryLinesToModelMapper
    {
        List<DeliveryLineModel> Map(IEnumerable<DeliveryLine> deliveryLines);
    }
}