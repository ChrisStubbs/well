namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IEpodImportService
    {
        void AddRoutesFile(RouteDelivery routeDelivery, int routeId);

        void AddRoutesEpodFile(RouteDelivery routeDelivery, int routesId);

        IEnumerable<RouteAttributeException> GetRouteAttributeException();

        void CopyFileToArchive(string filename, string fileNameWithoutPath, string archiveLocation);

        void AddAdamUpdateFile(RouteUpdates orderUpdates, int routesId);
    }
}
