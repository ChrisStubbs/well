namespace PH.Well.TranSend.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml.Serialization;
    using Common;
    using Common.Contracts;
    using Common.Extensions;
    using Contracts;
    using Domain;
    using Enums;
    using Repositories.Contracts;

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

            //var currentRouteImportFile = epodDomainImportService.GetByFileName(filenameWithoutPath);
            var currentRouteImportFile = epodDomainImportService.GetByFileName("a");


            if (currentRouteImportFile != null)
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

            RouteDeliveries routes = null;

            if (epodType == EpodFileType.RouteHeader)
            {
                
                var overrides = new XmlAttributeOverrides();
                var attribs = new XmlAttributes {XmlIgnore = true};
                attribs.XmlElements.Add(new XmlElementAttribute("RouteStatus"));
                overrides.Add(typeof(RouteHeader), "RouteStatus", attribs);

                attribs.XmlElements.Add(new XmlElementAttribute("RouteDate"));
                overrides.Add(typeof(RouteHeader), "RouteDate", attribs);

                var routeImportSerializer = new XmlSerializer(typeof(RouteDeliveries), overrides);
                var reader = new StreamReader(filename);

                routes = (RouteDeliveries)routeImportSerializer.Deserialize(reader);

                reader.Close();

                epodDomainImportService.CurrentUser = "ePodDomainImport";
                epodDomainImportService.AddRoutesFile(routes, routesId);
                logger.LogDebug($"File {filename} imported successfully");

            }

        }
    }
}
