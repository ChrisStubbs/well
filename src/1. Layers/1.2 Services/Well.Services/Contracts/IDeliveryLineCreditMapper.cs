namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface IDeliveryLineCreditMapper
    {
        List<DeliveryLineCredit> Map(Job job);
        List<DeliveryLineUplift> MapUplift(Job job);
    }
}