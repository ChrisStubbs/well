﻿namespace PH.Well.TranSend.Infrastructure
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
        private readonly IEpodImportService epodImportService;
        private readonly ILogger logger;


        public EpodFtpProvider(
            ILogger logger,
            IFtpClient ftpClient,
            IWebClient webClient,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IFileModule fileModule,
            IEpodImportService epodImportService
            )
        {
            this.logger = logger;
            this.ftpClient = ftpClient;
            this.webClient = webClient;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.fileModule = fileModule;
            this.epodImportService = epodImportService;

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
                var guid = Guid.NewGuid();
                var targetFileName = Path.Combine(Configuration.DownloadFilePath, listing.Filename);
                var tempFilename = GetTemporaryFilename(listing.Filename, guid);
               
                if (!File.Exists(targetFileName))
                {
                    var downloadedFile = this.webClient.CopyFile(Configuration.FtpLocation + "/" + listing.Filename,
                        Path.Combine(Configuration.DownloadFilePath, tempFilename));

                    if (string.IsNullOrWhiteSpace(downloadedFile))
                    {
                        this.logger.LogDebug($"Transend file not copied from FTP {listing.Filename}!");
                        this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"Transend file not copied from FTP {listing.Filename}!", EventId.FtpTransendFileNotCopied);

                        continue;
                    }
                    if (!File.Exists(targetFileName))
                    {
                        File.Move(downloadedFile, targetFileName);
                        this.logger.LogDebug($"Success! File {listing.Filename} copied from ftp!");
                    }

                    if (Configuration.DeleteFtpFileAfterImport)
                    {
                        this.ftpClient.DeleteFile(Path.GetFileName(targetFileName));
                    }
                    // Abort if a file called stop.txt exists in exe folder
                    if (File.Exists("stop.txt"))
                    {
                        File.Delete("stop.txt");
                        return;
                    }
                }
                else
                {
                    this.logger.LogDebug($"File { listing.Filename } already exists in target folder. File not copied from FTP.");
                }

            }
        }

        private string GetTemporaryFilename(string filename, Guid guid)
        {
            return $"{guid}{filename}";
        }
    }
}
