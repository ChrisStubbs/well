namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;
    public interface ICleanDeliveryRepository: IRepository<CleanDelivery, int>
    {
        IEnumerable<CleanDelivery> GetCleanDeliveries();
    }
}
