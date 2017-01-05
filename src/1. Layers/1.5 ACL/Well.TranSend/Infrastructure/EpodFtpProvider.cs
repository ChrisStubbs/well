namespace PH.Well.TranSend.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Xml.Serialization;

    using Contracts;
    using Common.Contracts;

    using PH.Well.Common;
    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    using Well.Services.Contracts;

    public class EpodFtpProvider : IEpodProvider
    {
        private readonly IFtpClient ftpClient;
        private readonly IWebClient webClient;
        private readonly IEventLogger eventLogger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IFileModule fileModule;
        private readonly ILogger logger;
        private readonly IEpodUpdateService epodUpdateService;

        public EpodFtpProvider(
            ILogger logger, 
            IEpodUpdateService epodUpdateService,
            IFtpClient ftpClient,
            IWebClient webClient,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IFileModule fileModule)
        {
            this.logger = logger;
            this.epodUpdateService = epodUpdateService;
            this.ftpClient = ftpClient;
            this.webClient = webClient;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.fileModule = fileModule;

            this.ftpClient.FtpLocation = Configuration.FtpLocation;
            this.ftpClient.FtpUserName = Configuration.FtpUsername;
            this.ftpClient.FtpPassword = Configuration.FtpPassword;
            this.webClient.Credentials = new NetworkCredential(Configuration.FtpUsername, Configuration.FtpPassword);
        }

        public void Import()
        {
            var listings = new List<DirectoryListing>();

            using (var response = this.ftpClient.GetResponseStream())
            {
                using (var reader = new StreamReader(response))
                {
                    var routeFile = string.Empty;

                    while ((routeFile = reader.ReadLine()) != null)
                    {
                        listings.Add(new DirectoryListing(routeFile));
                    }
                }
            }

            foreach (var listing in listings.OrderBy(x => x.Datetime))
            {
                var downloadedFile = this.webClient.CopyFile(Configuration.FtpLocation + "/" + listing.Filename, Configuration.DownloadFilePath + listing.Filename);

                if (string.IsNullOrWhiteSpace(downloadedFile))
                {
                    this.logger.LogDebug($"Transend file not copied from FTP {listing.Filename}!");
                    this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"Transend file not copied from FTP {listing.Filename}!", 4433);

                    continue;
                }

                var filename = Path.GetFileName(downloadedFile);

                var xmlSerializer = new XmlSerializer(typeof(RouteDelivery));

                this.logger.LogDebug($"Processing {filename}...");

                try
                {
                    using (var streamReader = new StreamReader(downloadedFile))
                    {
                        var routes = (RouteDelivery)xmlSerializer.Deserialize(streamReader);

                        var route = this.routeHeaderRepository.Create(new Routes { FileName = filename });

                        routes.RouteId = route.Id;

                        this.epodUpdateService.Update(routes);
                    }

                    this.ftpClient.DeleteFile(filename);
                    this.fileModule.MoveFile(downloadedFile, Configuration.ArchiveLocation);

                    logger.LogDebug($"File {listing.Filename} imported!");
                }
                catch (Exception exception)
                {
                    this.logger.LogError($"Epod update error in XML!", exception);
                }
            }
        }
    }
}
