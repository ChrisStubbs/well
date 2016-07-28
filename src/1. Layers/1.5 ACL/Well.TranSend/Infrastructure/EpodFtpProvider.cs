namespace PH.Well.TranSend.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using Contracts;
    using System.Configuration;
    using Common.Contracts;
    using Common.Extensions;
    using Well.Services.Contracts;
    using static System.String;

    public class EpodFtpProvider : IEpodProvider
    {
        private string ftpLocation;
        private string ftpUser;
        private string ftpPass;
        private string networkUser;
        private string networkUserPass;
        private string archiveLocation;
        private readonly IEpodSchemaProvider epodSchemaProvider;
        private readonly IEpodDomainImportProvider epodDomainImportProvider;
        private readonly IEpodDomainImportService epodDomainImportService;
        private readonly IEpodImportConfiguration config;
        private readonly ILogger logger;
        private readonly string correctExtension = ".xml";
        private readonly string assemblyName = "PH.Well.TranSend";

        public EpodFtpProvider(IEpodSchemaProvider epodSchemaProvider, ILogger logger, IEpodDomainImportProvider epodDomainImportProvider,
            IEpodDomainImportService epodDomainImportService, IEpodImportConfiguration config)
        {
            this.epodSchemaProvider = epodSchemaProvider;
            this.logger = logger;
            this.epodDomainImportProvider = epodDomainImportProvider;
            this.epodDomainImportService = epodDomainImportService;
            this.config = config;
        }

        public void ListFilesAndProcess(List<string> schemaErrors)
        {

            this.archiveLocation = config.ArchiveLocation;
            this.ftpLocation = config.FtpLocation;
            this.ftpUser = config.FtpUser;
            this.ftpPass = config.FtpPass;
            this.networkUser = config.NetworkUser;
            this.networkUserPass = config.NetworkUserPass;

            var request = (FtpWebRequest) WebRequest.Create(ftpLocation);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(ftpUser, ftpPass);
            var response = (FtpWebResponse) request.GetResponse();
            var responseStream = response.GetResponseStream();
            schemaErrors = new List<string>();

            if (responseStream == null)
                throw new Exception("response stream is null");

            var reader = new StreamReader(responseStream);

            while (!reader.EndOfStream)
            {
                var routeFile = reader.ReadLine();
                string downloadedFile;
                DownLoadFileFromFtp(routeFile, out downloadedFile);
                
                if(downloadedFile == Empty)
                    throw new Exception("error with file download");

                var filenameWithoutPath = downloadedFile.GetFilenameWithoutPath();

                if (routeFile != null && epodDomainImportService.IsFileXmlType(downloadedFile))
                {
                    var fileTypeIndentifier = epodDomainImportService.GetFileTypeIdentifier(filenameWithoutPath);
                    var schemaName = epodDomainImportService.MatchFileNameToSchema(fileTypeIndentifier);
                    var schemaPath = epodDomainImportService.GetSchemaFilePath(schemaName);
                    var validationErrors = new List<string>();
        {

            this.archiveLocation = config.ArchiveLocation;
            this.ftpLocation = config.FtpLocation;
            this.ftpUser = config.FtpUser;
            this.ftpPass = config.FtpPass;
            this.networkUser = config.NetworkUser;
            this.networkUserPass = config.NetworkUserPass;

            var request = (FtpWebRequest) WebRequest.Create(ftpLocation);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(ftpUser, ftpPass);
            var response = (FtpWebResponse) request.GetResponse();
            var responseStream = response.GetResponseStream();
            schemaErrors = new List<string>();

            if (responseStream == null)
                throw new Exception("response stream is null");

            var reader = new StreamReader(responseStream);

            while (!reader.EndOfStream)
            {
                var routeFile = reader.ReadLine();
                string downloadedFile;
                DownLoadFileFromFtp(routeFile, out downloadedFile);
                
                if(downloadedFile == Empty)
                    throw new Exception("error with file download");

                var filenameWithoutPath = downloadedFile.GetFilenameWithoutPath();

                if (routeFile != null && epodDomainImportService.IsFileXmlType(downloadedFile))
                {
                    var fileTypeIndentifier = epodDomainImportService.GetFileTypeIdentifier(filenameWithoutPath);
                    var schemaName = epodDomainImportService.MatchFileNameToSchema(fileTypeIndentifier);
                    var schemaPath = epodDomainImportService.GetSchemaFilePath(schemaName);
                    var validationErrors = new List<string>();
                    var isFileValidBySchema = epodSchemaProvider.IsFileValid(downloadedFile, schemaPath, ref  validationErrors);

                    if (!isFileValidBySchema)
                    {
                        var validationError = $"file {routeFile} failed schema validation with the following: {string.Join(",", validationErrors)}";
                        logger.LogError(validationError);
                    }
                    else
                    {
                        var epodType = epodDomainImportService.GetEpodFileType(fileTypeIndentifier);
                        epodDomainImportProvider.ImportRouteHeader(downloadedFile, epodType);
                        epodDomainImportService.CopyFileToArchive(downloadedFile, filenameWithoutPath, this.archiveLocation);
                        DeleteFileOnFtpServer(new Uri(this.ftpLocation + filenameWithoutPath), this.ftpUser, this.ftpPass);
                        logger.LogDebug($"File {routeFile} imported.");
                    }              
                }
            }

            response.Close();
            responseStream.Close();
            reader.Close();
        }



        private bool DeleteFileOnFtpServer(Uri serverUri, string ftpUsername, string ftpPassword)
        {
            try
            {
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    return false;
                }

                var request = (FtpWebRequest)WebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                request.Method = WebRequestMethods.Ftp.DeleteFile;

                var response = (FtpWebResponse)request.GetResponse();
                return response.StatusCode == FtpStatusCode.FileActionOK;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void DownLoadFileFromFtp(string routeFile, out string downloadedFilePath)
        {
            var downloadFilePath = ConfigurationManager.AppSettings["downloadFilePath"] + routeFile;
            var fullFtpPath = this.ftpLocation + routeFile;
            var fileDownloaded = false;

            using (var downloadRequest = new WebClient())
            {
                downloadRequest.Credentials = new NetworkCredential(this.ftpUser, this.ftpPass);
                var filedata = downloadRequest.DownloadData(fullFtpPath);

                using (var file = File.Create(downloadFilePath))
                {
                    file.Write(filedata, 0, filedata.Length);
                    file.Close();
                    downloadedFilePath = file.Name;
                }
            }
        }

 

    }
}
