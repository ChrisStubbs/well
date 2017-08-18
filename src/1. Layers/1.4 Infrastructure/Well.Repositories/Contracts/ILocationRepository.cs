namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface ILocationRepository
    {
        SingleLocation GetSingleLocationById(int id);
        SingleLocation GetSingleLocation(int? locationId, string accountNumber = null, int? branchId = null);
        IList<Location> GetLocation(int branchId);
    }
}
