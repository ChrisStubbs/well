namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;

    public interface IAdamImportService
    {
        void Import(RouteDelivery route);
    }
}