namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using Domain.ValueObjects;

    public interface IDeliveryService
    {
        IEnumerable<CleanDelivery> GetCleanDeliveries();
    }
}
