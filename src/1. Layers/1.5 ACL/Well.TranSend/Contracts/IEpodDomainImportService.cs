namespace PH.Well.TranSend.Contracts
{
    using Domain;
    using Enums;

    public interface IEpodDomainImportService
    {
        Routes GetByFileName(string filename);
        Routes CreateOrUpdate(Routes routes);

        string CurrentUser { get; set; }

        EpodFileType EpodType { get; set; }

        void AddRoutesFile(RouteDeliveries routeDeliveries, int routesId);

        void AddRoutesEpodFile(RouteDeliveries routeDeliveries, int routesId);

    }
}
