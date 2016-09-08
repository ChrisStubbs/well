namespace PH.Well.Services.EpodImport
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using Common.Contracts;
    using Common.Extensions;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Services.Contracts;

    public class EpodDomainImportProvider : IEpodDomainImportProvider
    {
        private readonly IEpodDomainImportService epodDomainImportService;
        private readonly ILogger logger;

        public EpodDomainImportProvider(IEpodDomainImportService eopEpodDomainImportService, ILogger logger)
        {
            this.epodDomainImportService = eopEpodDomainImportService;
            this.logger = logger;
        }

        public void ImportRouteHeader(string filename, EpodFileType fileType)
        {
            var filenameWithoutPath = filename.GetFilenameWithoutPath();

            var currentRouteImportFile = new Routes();

            if (fileType == EpodFileType.RouteHeader)
            {
                currentRouteImportFile = epodDomainImportService.GetByFileName(filenameWithoutPath);
            }

            if (currentRouteImportFile != null && fileType == EpodFileType.RouteHeader)
            {
                logger.LogError($"file {filenameWithoutPath} has already been imported");
                throw new Exception("error with file download");
            }
            else
            {
                epodDomainImportService.CurrentUser = "ePodDomainImport";
                var routes = epodDomainImportService.CreateOrUpdate(new Routes { FileName = filenameWithoutPath });
                MapRoutesToDomain(filename, fileType, routes.Id);
               
            }
        }

        private void MapRoutesToDomain(string filename, EpodFileType epodType, int routesId)
        {
            var overrides = new XmlAttributeOverrides();
            var attribs = new XmlAttributes { XmlIgnore = true };

            if (epodType == EpodFileType.RouteHeader)
            {

                var attributesExceptions = this.epodDomainImportService.GetRouteAttributeException();

                foreach (var attributesException in attributesExceptions)
                {
                    attribs.XmlElements.Add(new XmlElementAttribute(attributesException.AttributeName));
                    overrides.Add(typeof(RouteHeader), attributesException.AttributeName, attribs);
                }
            }
            else
            {
                attribs.XmlElements.Add(new XmlElementAttribute("EntityAttributeValues"));
                overrides.Add(typeof(RouteHeader), "EntityAttributeValues", attribs);
            }

            var reader = new StreamReader(filename);
            epodDomainImportService.EpodType = epodType;
            epodDomainImportService.CurrentUser = "ePodDomainImport";
            var filnameWithoutPath = filename.GetFilenameWithoutPath();
            var archiveLocation = System.Configuration.ConfigurationManager.AppSettings["archiveLocation"];

            if (epodType == EpodFileType.RouteHeader || epodType == EpodFileType.RouteEpod)
            {
                var routeImportSerializer = new XmlSerializer(typeof(RouteDeliveries), overrides);
                var routes = (RouteDeliveries) routeImportSerializer.Deserialize(reader);

               
                if (epodType == EpodFileType.RouteHeader)
                {
                    epodDomainImportService.AddRoutesFile(routes, routesId);
                }
                else
                {
                    epodDomainImportService.AddRoutesEpodFile(routes, routesId);           
                }
            }
            else
            {
                var adamUpdatesSerializer = new XmlSerializer(typeof(RouteUpdates), overrides);
                var orderUpdates = (RouteUpdates)adamUpdatesSerializer.Deserialize(reader);
                epodDomainImportService.AddAdamUpdateFile(orderUpdates, routesId);
            }



            reader.Close();

            if (epodType == EpodFileType.RouteEpod || epodType == EpodFileType.OrderUpdate)
                epodDomainImportService.CopyFileToArchive(filename, filnameWithoutPath, archiveLocation);



            logger.LogDebug($"File {filename} imported successfully");

        }



    }
}
