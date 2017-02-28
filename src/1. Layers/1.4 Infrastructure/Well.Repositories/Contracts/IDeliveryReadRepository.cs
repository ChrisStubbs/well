namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IDeliveryReadRepository
    {
        IEnumerable<Delivery> GetCleanDeliveries(string username);

        IEnumerable<Delivery> GetResolvedDeliveries(string username);

        IEnumerable<Delivery> GetExceptionDeliveries(string username, bool includePendingCredit = false);

        IEnumerable<DeliveryLine> GetDeliveryLinesByJobId(int id);

        DeliveryDetail GetDeliveryById(int id, string username);

        IEnumerable<Delivery> GetByPendingCredit(string username);
    }
}