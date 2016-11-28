namespace PH.Well.TranSend.Infrastructure
{
    using System;
    using System.IO;
    using System.Net;
    using System.Xml.Serialization;

    using Contracts;
    using Common.Contracts;

    using PH.Well.Common;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    using Well.Services.Contracts;

    public class EpodFtpProvider : IEpodProvider
    {
        private readonly IFtpClient ftpClient;
        private readonly IWebClient webClient;
        private readonly IEventLogger eventLogger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly ILogger logger;
        private readonly IEpodUpdateService epodUpdateService;

        public EpodFtpProvider(
            ILogger logger, 
            IEpodUpdateService epodUpdateService,
            IFtpClient ftpClient,
            IWebClient webClient,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository)
        {
            this.logger = logger;
            this.epodUpdateService = epodUpdateService;
            this.ftpClient = ftpClient;
            this.webClient = webClient;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;

            this.ftpClient.FtpLocation = Configuration.FtpLocation;
            this.ftpClient.FtpUserName = Configuration.FtpUsername;
            this.ftpClient.FtpPassword = Configuration.FtpPassword;
            this.webClient.Credentials = new NetworkCredential(Configuration.FtpUsername, Configuration.FtpPassword);
        }

        public void Import()
        {
            using (var response = this.ftpClient.GetResponseStream())
            {
                using (var reader = new StreamReader(response))
                {
                    var routeFile = string.Empty;

                    while ((routeFile = reader.ReadLine()) != null)
                    {
                        if (!routeFile.EndsWith("xml"))
                            continue;

                        // TODO copy all files over first with the date created of the file as we want to load the transend files in order oldest first

                        var downloadedFile = this.webClient.CopyFile(Configuration.FtpLocation + "/" + routeFile, Configuration.DownloadFilePath + routeFile);

                        if (string.IsNullOrWhiteSpace(downloadedFile))
                        {
                            this.logger.LogDebug($"Transend file not copied from FTP {routeFile}!");
                            this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"Transend file not copied from FTP {routeFile}!", 4433);

                            continue;
                        }
                        
                        var filename = Path.GetFileName(downloadedFile);

                        var xmlSerializer = new XmlSerializer(typeof(RouteDelivery));

                        try
                        {
                            using (var streamReader = new StreamReader(downloadedFile))
                            {
                                var routes = (RouteDelivery)xmlSerializer.Deserialize(streamReader);

                                var route = this.routeHeaderRepository.Create(new Routes { FileName = filename });

                                routes.RouteId = route.Id;

                                this.epodUpdateService.Update(routes);
                            }
                        }
                        catch (Exception exception)
                        {
                            this.logger.LogError($"Epod update error in XML!", exception);
                        }

                        this.ftpClient.DeleteFile(filename);

                        logger.LogDebug($"File {routeFile} imported!");
                    }
                }
            }
        }
    }
}
