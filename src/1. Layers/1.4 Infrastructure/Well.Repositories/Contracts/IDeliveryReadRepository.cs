namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IDeliveryReadRepository
    {
        IEnumerable<Delivery> GetCleanDeliveries(string userName);
        IEnumerable<Delivery> GetResolvedDeliveries(string userName);
        IEnumerable<Delivery> GetExceptionDeliveries(string userName);
    }
}