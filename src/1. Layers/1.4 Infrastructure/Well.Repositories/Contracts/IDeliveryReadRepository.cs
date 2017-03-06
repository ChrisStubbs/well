namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.Enums;
    using Domain.ValueObjects;

    public interface IDeliveryReadRepository
    {
        IEnumerable<Delivery> GetByStatus(string username, JobStatus jobStatus);

        IEnumerable<DeliveryLine> GetDeliveryLinesByJobId(int id);

        DeliveryDetail GetDeliveryById(int id, string username);
    }
}