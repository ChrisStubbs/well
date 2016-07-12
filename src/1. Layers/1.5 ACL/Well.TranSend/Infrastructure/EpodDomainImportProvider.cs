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
            var overrides = new XmlAttributeOverrides();
            var attribs = new XmlAttributes { XmlIgnore = true };

            if (epodType == EpodFileType.RouteHeader)
            {
              
                attribs.XmlElements.Add(new XmlElementAttribute("RouteStatusCode"));
                overrides.Add(typeof(RouteHeader), "RouteStatusCode", attribs);

                attribs.XmlElements.Add(new XmlElementAttribute("PerformanceStatusCode"));
                overrides.Add(typeof(RouteHeader), "PerformanceStatusCode", attribs);

                attribs.XmlElements.Add(new XmlElementAttribute("LastRouteUpdate"));
                overrides.Add(typeof(RouteHeader), "LastRouteUpdate", attribs);

                attribs.XmlElements.Add(new XmlElementAttribute("AuthByPass"));
                overrides.Add(typeof(RouteHeader), "AuthByPass", attribs);

                attribs.XmlElements.Add(new XmlElementAttribute("NonAuthByPass"));
                overrides.Add(typeof(RouteHeader), "NonAuthByPass", attribs);

                attribs.XmlElements.Add(new XmlElementAttribute("ShortDeliveries"));
                overrides.Add(typeof(RouteHeader), "ShortDeliveries", attribs);

                attribs.XmlElements.Add(new XmlElementAttribute("DamagesRejected"));
                overrides.Add(typeof(RouteHeader), "DamagesRejected", attribs);

                attribs.XmlElements.Add(new XmlElementAttribute("DamagesAccepted"));
                overrides.Add(typeof(RouteHeader), "DamagesAccepted", attribs);

                attribs.XmlElements.Add(new XmlElementAttribute("NotRequired"));
                overrides.Add(typeof(RouteHeader), "NotRequired", attribs);

                attribs.XmlElements.Add(new XmlElementAttribute("Depot"));
                overrides.Add(typeof(RouteHeader), "Depot", attribs);


                attribs.XmlElements.Add(new XmlElementAttribute("RouteDate"));
                overrides.Add(typeof(RouteHeader), "RouteDate", attribs);
            }
            else
            {
                
            }

            var routeImportSerializer = new XmlSerializer(typeof(RouteDeliveries), overrides);
            var reader = new StreamReader(filename);

            var routes = (RouteDeliveries)routeImportSerializer.Deserialize(reader);

            reader.Close();

            epodDomainImportService.EpodType = epodType;
            epodDomainImportService.CurrentUser = "ePodDomainImport";
            epodDomainImportService.AddRoutesFile(routes, routesId);
            logger.LogDebug($"File {filename} imported successfully");

        }
    }
}
