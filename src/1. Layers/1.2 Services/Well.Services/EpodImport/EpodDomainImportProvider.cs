namespace PH.Well.Services.EpodImport
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Xml.Serialization;
    using Common.Contracts;
    using Common.Extensions;
    using Domain;
    using Domain.Enums;

    using PH.Well.Repositories.Contracts;

    using Services.Contracts;

    public class EpodDomainImportProvider : IEpodDomainImportProvider
    {
        private readonly IEpodDomainImportService epodDomainImportService;
        private readonly ILogger logger;
        private readonly IRouteHeaderRepository routeHeaderRepository;

        public EpodDomainImportProvider(IEpodDomainImportService eopEpodDomainImportService, ILogger logger, IRouteHeaderRepository routeHeaderRepository)
        {
            this.epodDomainImportService = eopEpodDomainImportService;
            this.logger = logger;
            this.routeHeaderRepository = routeHeaderRepository;
        }

        public void ImportRouteHeader(string filename, EpodFileType fileType)
        {
            var filenameWithoutPath = filename.GetFilenameWithoutPath();

            var currentRouteImportFile = new Routes();

            if (fileType == EpodFileType.RouteHeader)
            {
                currentRouteImportFile = this.routeHeaderRepository.GetByFilename(filenameWithoutPath);
            }

            if (currentRouteImportFile != null && fileType == EpodFileType.RouteHeader)
            {
                logger.LogError($"file {filenameWithoutPath} has already been imported");
                throw new Exception("error with file download");
            }
            else
            {
                this.routeHeaderRepository.CurrentUser = "ePodDomainImport";
                var routes = this.routeHeaderRepository.CreateOrUpdate(new Routes { FileName = filenameWithoutPath });
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

            using (var reader = new StreamReader(filename))
            {
                epodDomainImportService.EpodType = epodType;
                
                if (epodType == EpodFileType.RouteHeader || epodType == EpodFileType.RouteEpod)
                {
                    var routeImportSerializer = new XmlSerializer(typeof(RouteDelivery), overrides);
                    var routes = (RouteDelivery)routeImportSerializer.Deserialize(reader);

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
            }

            var filnameWithoutPath = filename.GetFilenameWithoutPath();
            var archiveLocation = ConfigurationManager.AppSettings["archiveLocation"];

            if (epodType == EpodFileType.RouteEpod || epodType == EpodFileType.OrderUpdate)
                epodDomainImportService.CopyFileToArchive(filename, filnameWithoutPath, archiveLocation);
            
            logger.LogDebug($"File {filename} imported successfully");
        }
    }
}
