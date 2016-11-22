namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;

    public interface IEpodUpdateService
    {
        void Update(RouteDelivery route);
    }
}