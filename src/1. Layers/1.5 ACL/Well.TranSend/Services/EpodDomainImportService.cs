namespace PH.Well.TranSend.Services
{
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Repositories.Contracts;

    public class EpodDomainImportService : IEpodDomainImportService
    {
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly ILogger logger;
        public string CurrentUser { get; set; }

        public EpodDomainImportService(IRouteHeaderRepository routeHeaderRepository, ILogger logger)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.logger = logger;
        }


        public Routes GetByFileName(string filename)
        {
            return this.routeHeaderRepository.GetByFilename(filename);
        }

        public Routes CreateOrUpdate(Routes routes)
        {
            routeHeaderRepository.CurrentUser = CurrentUser;
            return this.routeHeaderRepository.CreateOrUpdate(routes);
        }

        public void AddRoutesFile(RouteDeliveries routeDeliveries, int routesId)
        {

            foreach (var routeHeader in routeDeliveries.RouteHeaders)
            {
                routeHeader.RoutesId = routesId;
                this.routeHeaderRepository.RouteHeaderCreateOrUpdate(routeHeader);
            }

        }

    }
}
