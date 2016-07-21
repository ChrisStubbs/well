namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IDeliveryReadRepository
    {
        IEnumerable<Delivery> GetCleanDeliveries();
        IEnumerable<Delivery> GetResolvedDeliveries();
        IEnumerable<Delivery> GetExceptionDeliveries();
    }
}