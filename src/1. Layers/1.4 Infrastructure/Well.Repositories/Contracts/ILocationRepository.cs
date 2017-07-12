namespace PH.Well.Repositories.Contracts
{
    using Domain;

    public interface ILocationRepository
    {
        SingleLocation GetSingleLocation(int? locationId, string accountNumber = null, int? branchId = null);
    }
}
