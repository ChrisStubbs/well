namespace PH.Well.Services.Contracts
{
    using Domain;

    public interface IEpodImportService
    {
        void Import(RouteDelivery route, string fileName, out bool hasErrors);
        bool TryImportRouteHeader(RouteHeader header, string fileName);
    }
}