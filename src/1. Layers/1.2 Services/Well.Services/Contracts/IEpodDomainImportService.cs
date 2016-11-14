namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;

    public interface IEpodDomainImportService
    {
        EpodFileType EpodType { get; set; }

        void AddRoutesFile(RouteDelivery routeDelivery, int routesId);

        void AddRoutesEpodFile(RouteDelivery routeDelivery, int routesId);

        string MatchFileNameToSchema(string fileTypeIndentifier);

        string GetFileTypeIdentifier(string filename);

        EpodFileType GetEpodFileType(string fileTypeIndentifier);

        string GetSchemaFilePath(string schemaName);

        IEnumerable<RouteAttributeException> GetRouteAttributeException();

        void CopyFileToArchive(string filename, string fileNameWithoutPath, string archiveLocation);

        void AddAdamUpdateFile(RouteUpdates orderUpdates, int routesId);
    }
}
