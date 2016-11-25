namespace PH.Well.Services.EpodImport
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Xml.Serialization;
    using Common.Contracts;
    using Domain;
    using Domain.Enums;
    using PH.Well.Common;
    using PH.Well.Repositories.Contracts;
    using Services.Contracts;

    public class EpodImportProvider : IEpodImportProvider
    {
        private readonly IEpodImportService epodImportService;
        private readonly ILogger logger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IFileTypeService fileTypeService;
        private readonly IEventLogger eventLogger;

        public EpodImportProvider(
            IEpodImportService eopEpodImportService,
            ILogger logger,
            IRouteHeaderRepository routeHeaderRepository,
            IFileTypeService fileTypeService,
            IEventLogger eventLogger)
        {
            this.epodImportService = eopEpodImportService;
            this.logger = logger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.fileTypeService = fileTypeService;
            this.eventLogger = eventLogger;
        }

        public void ImportRouteHeader(string fullpathFilename)
        {
            var filename = Path.GetFileName(fullpathFilename);

            var fileType = this.fileTypeService.DetermineFileType(filename);

            if (fileType == EpodFileType.AdamInsert)
            {
                var alreadyLoaded = this.routeHeaderRepository.FileAlreadyLoaded(filename);

                if (alreadyLoaded)
                {
                    logger.LogDebug($"File ({filename}) has already been imported!");
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"File ({filename}) has already been imported!",
                        1049);
                    return;
                }
            }

            this.routeHeaderRepository.CurrentUser = "ePodImport";
            var route = this.routeHeaderRepository.Create(new Routes { FileName = filename });
            this.MapRoutesToDomain(fullpathFilename, fileType, route.Id);
        }

        private void MapRoutesToDomain(string filename, EpodFileType epodType, int routeId)
        {
            /*var overrides = new XmlAttributeOverrides();
            var attribs = new XmlAttributes { XmlIgnore = true };

            // TODO not sure if we need this
            if (epodType == EpodFileType.RouteHeader)
            {
                var attributesExceptions = this.epodImportService.GetRouteAttributeException();

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
            }*/

            using (var reader = new StreamReader(filename))
            {
                if (epodType == EpodFileType.AdamInsert || epodType == EpodFileType.EpodUpdate)
                {
                    var routeImportSerializer = new XmlSerializer(typeof(RouteDelivery));

                    var routes = new RouteDelivery();

                    try
                    {
                        routes = (RouteDelivery)routeImportSerializer.Deserialize(reader);
                    }
                    catch (Exception exception)
                    {
                        this.logger.LogError(
                            $"File can not be deserialised to route delivery! Incorrect xml! {filename}",
                            exception);
                        this.eventLogger.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"File can not be deserialised to route delivery! Incorrect xml! {filename}",
                            2165);

                        return;
                    }

                    if (epodType == EpodFileType.AdamInsert)
                    {
                        this.epodImportService.AddRoutesFile(routes, routeId);
                    }
                    else
                    {
                        this.epodImportService.AddRoutesEpodFile(routes, routeId);
                    }
                }
                else // we have a update from adam
                {
                    var adamUpdatesSerializer = new XmlSerializer(typeof(RouteUpdates));

                    var orderUpdates = new RouteUpdates();

                    try
                    {
                        orderUpdates = (RouteUpdates)adamUpdatesSerializer.Deserialize(reader);
                    }
                    catch (Exception exception)
                    {
                        this.logger.LogError(
                            $"File can not be deserialised to route update! Incorrect xml! {filename}",
                            exception);
                        this.eventLogger.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"File can not be deserialised to route update! Incorrect xml! {filename}",
                            2166);

                        return;
                    }
                    
                    this.epodImportService.AddAdamUpdateFile(orderUpdates, routeId);
                }
            }

            var filnameWithoutPath = Path.GetFileName(filename);
            var archiveLocation = ConfigurationManager.AppSettings["archiveLocation"];

            this.epodImportService.CopyFileToArchive(filename, filnameWithoutPath, archiveLocation);
            
            logger.LogDebug($"File {filename} imported successfully");
        }
    }
}
