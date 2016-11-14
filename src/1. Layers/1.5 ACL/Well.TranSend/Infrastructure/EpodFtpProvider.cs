namespace PH.Well.TranSend.Infrastructure
{
    using System;
    using System.IO;
    using System.Net;
    using Contracts;
    using Common.Contracts;

    using PH.Well.Common;

    using Well.Services.Contracts;

    public class EpodFtpProvider : IEpodProvider
    {
        private readonly IEpodSchemaValidator epodSchemaValidator;
        private readonly IEpodDomainImportProvider epodDomainImportProvider;
        private readonly IEpodDomainImportService epodDomainImportService;
        private readonly IFtpClient ftpClient;
        private readonly IWebClient webClient;
        private readonly IEventLogger eventLogger;
        private readonly ILogger logger;

        public EpodFtpProvider(IEpodSchemaValidator epodSchemaValidator, 
            ILogger logger, 
            IEpodDomainImportProvider epodDomainImportProvider,
            IEpodDomainImportService epodDomainImportService,
            IFtpClient ftpClient,
            IWebClient webClient,
            IEventLogger eventLogger)
        {
            this.epodSchemaValidator = epodSchemaValidator;
            this.logger = logger;
            this.epodDomainImportProvider = epodDomainImportProvider;
            this.epodDomainImportService = epodDomainImportService;
            this.ftpClient = ftpClient;
            this.webClient = webClient;
            this.eventLogger = eventLogger;

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

                        var downloadedFile = this.webClient.CopyFile(Configuration.FtpLocation + "/" + routeFile, Configuration.DownloadFilePath + routeFile);

                        if (string.IsNullOrWhiteSpace(downloadedFile))
                        {
                            this.logger.LogDebug($"Transend file not copied from FTP {routeFile}!");
                            this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"Transend file not copied from FTP {routeFile}!", 4433);

                            continue;
                        }
                        
                        var filename = Path.GetFileName(downloadedFile);

                        var fileTypeIndentifier = epodDomainImportService.GetFileTypeIdentifier(filename);

                        if (string.IsNullOrWhiteSpace(fileTypeIndentifier))
                        {
                            this.logger.LogDebug($"File incorrect {filename}! File name should start with one of the following: (PH_) (ePod_) (PHOrder_)");
                            this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"File incorrect {filename}! File name should start with one of the following: (PH_) (ePod_) (PHOrder_)", 8833);

                            continue;
                        }

                        var schemaName = epodDomainImportService.MatchFileNameToSchema(fileTypeIndentifier);
                        var schemaPath = epodDomainImportService.GetSchemaFilePath(schemaName);

                        var isFileValidBySchema = this.epodSchemaValidator.IsFileValid(downloadedFile, schemaPath);

                        if (isFileValidBySchema)
                        {
                            var epodType = epodDomainImportService.GetEpodFileType(fileTypeIndentifier);

                            epodDomainImportProvider.ImportRouteHeader(downloadedFile, epodType);

                            epodDomainImportService.CopyFileToArchive(downloadedFile, filename, Configuration.ArchiveLocation);

                            this.ftpClient.DeleteFile(filename);

                            logger.LogDebug($"File {routeFile} imported!");
                        }
                    }
                }
            }
        }
    }
}
