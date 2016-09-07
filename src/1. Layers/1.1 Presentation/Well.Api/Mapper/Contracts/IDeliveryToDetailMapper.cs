namespace PH.Well.Api.Mapper.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Api.Models;
    using PH.Well.Domain.ValueObjects;

    public interface IDeliveryToDetailMapper
    {
        DeliveryDetailModel Map(IEnumerable<DeliveryLine> lines, DeliveryDetail detail);
    }
}