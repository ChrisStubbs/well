namespace PH.Well.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    public interface IEpodDomainImportService
    {
        Routes GetByFileName(string filename);
        Routes CreateOrUpdate(Routes routes);

        string CurrentUser { get; set; }
        EpodFileType EpodType { get; set; }

        void AddRoutesFile(RouteDeliveries routeDeliveries, int routesId);

        void AddRoutesEpodFile(RouteDeliveries routeDeliveries, int routesId);

        string MatchFileNameToSchema(string fileTypeIndentifier);

        string GetFileTypeIdentifier(string filename);

        EpodFileType GetEpodFileType(string fileTypeIndentifier);

        string GetSchemaFilePath(string schemaName);

        bool IsFileXmlType(string fileName);

        IEnumerable<RouteAttributeException> GetRouteAttributeException();

        void CopyFileToArchive(string filename, string fileNameWithoutPath, string archiveLocation);

        void AddAdamUpdateFile(RouteUpdates orderUpdates, int routesId);
    }
}
