namespace PH.Well.TranSend.Contracts
{
    using Domain;

    public interface IEpodDomainImportService
    {
        Routes GetByFileName(string filename);
        Routes CreateOrUpdate(Routes routes);

        string CurrentUser { get; set; }

        void AddRoutesFile(RouteDeliveries routeDeliveries, int routesId);

    }
}
