namespace PH.Well.TranSend.Infrastructure
{
    using System;
    using System.IO;
    using System.Net;
    using Contracts;
    using System.Configuration;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
    using Enums;
    using static System.String;

    public class EpodFtpProvider : IEpodFtpProvider
    {
        private string ftpLocation;
        private string ftpUser;
        private string ftpPass;
        private string networkUser;
        private string networkUserPass;
        private readonly IEpodSchemaProvider epodSchemaProvider;
        private readonly IEpodDomainImportProvider epodDomainImportProvider;
        private readonly ILogger logger;
        private readonly string correctExtension = ".xml";
        private readonly string assemblyName = "PH.Well.TranSend";



        public EpodFtpProvider(IEpodSchemaProvider epodSchemaProvider, ILogger logger, IEpodDomainImportProvider epodDomainImportProvider)
        {
            this.epodSchemaProvider = epodSchemaProvider;
            this.logger = logger;
            this.epodDomainImportProvider = epodDomainImportProvider;
        }


        public void ListFilesAndProcess()
        {

            this.ftpLocation = ConfigurationManager.AppSettings["transendFTPLocation"];
            this.ftpUser = ConfigurationManager.AppSettings["transendUser"];
            this.ftpPass = ConfigurationManager.AppSettings["transendPass"];
            this.networkUser = ConfigurationManager.AppSettings["localUser"];
            this.networkUserPass = ConfigurationManager.AppSettings["localUserPass"];

            var request = (FtpWebRequest) WebRequest.Create(ftpLocation);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(ftpUser, ftpPass);
            var response = (FtpWebResponse) request.GetResponse();
            var responseStream = response.GetResponseStream();
            var schemaErrors = Empty;

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

                if (routeFile != null && IsFileXmlType(downloadedFile))
                {
                    var fileTypeIndentifier = GetFileTypeIdentifier(filenameWithoutPath);
                    var schemaName = MatchFileNameToSchema(fileTypeIndentifier);
                    var schemaPath = GetSchemaFilePath(schemaName);
                    var isFileValidBySchema = epodSchemaProvider.IsFileValid(downloadedFile, schemaPath);

                    if (!isFileValidBySchema)
                    {
                        logger.LogError($"file {routeFile} failed schema validation");
                    }
                    else
                    {
                        var epodType = GetEpodFileType(fileTypeIndentifier);
                        epodDomainImportProvider.ImportRouteHeader(downloadedFile, epodType);
                    }              
                }
            }

            response.Close();
            responseStream.Close();
            reader.Close();
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


        private bool IsFileXmlType(string fileName)
        {
            return Path.GetExtension(fileName) == correctExtension;
        }

        private static string MatchFileNameToSchema(string fileTypeIndentifier)
        {
            var fileType = GetEpodFileType(fileTypeIndentifier);

            return StringExtensions.GetEnumDescription(fileType == EpodFileType.RouteHeader ? TransendSchemaType.RouteHeaderSchema : TransendSchemaType.RouteEpodSchema);
        }

        private static string GetFileTypeIdentifier(string filename)
        {
            var position = filename.IndexOf("_", StringComparison.Ordinal);
            return filename.Substring(0, position + 1);
        }

        private static EpodFileType GetEpodFileType(string fileTypeIndentifier)
        {
            return StringExtensions.GetValueFromDescription<EpodFileType>(fileTypeIndentifier);
        }

        private string GetSchemaFilePath(string schemaName)
        {
            var bundleAssembly = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName.Contains(assemblyName));
            var asmPath = new Uri(bundleAssembly.CodeBase).LocalPath;
            return Path.Combine(Path.GetDirectoryName(asmPath), "Schemas", schemaName);
        }


    }
}
