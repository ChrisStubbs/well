namespace PH.Well.Services.Contracts
{
    using Domain;

    public interface IEpodImportService
    {
        void Import(RouteDelivery route, string fileName);
        void ImportRouteHeader(RouteHeader header, string fileName);
    }
}