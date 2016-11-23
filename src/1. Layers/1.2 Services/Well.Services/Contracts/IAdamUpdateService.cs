namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;

    public interface IAdamUpdateService
    {
        void Update(RouteUpdates route);
    }
}