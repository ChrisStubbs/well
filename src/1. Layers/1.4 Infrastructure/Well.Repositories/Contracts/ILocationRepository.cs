namespace PH.Well.Repositories.Contracts
{
    using Domain;

    public interface ILocationRepository
    {
        Location GetLocationById(int locationId);
    }
}
