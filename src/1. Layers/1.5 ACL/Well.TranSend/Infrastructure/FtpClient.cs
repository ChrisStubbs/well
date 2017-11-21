namespace PH.Well.Common
{
    using System;
    using System.Diagnostics.Eventing.Reader;
    using System.IO;
    using System.Net;

    using PH.Well.Common.Contracts;

    public class FtpClient : IFtpClient
    {
        private readonly ILogger logger;

        private readonly IEventLogger eventLogger;

        public FtpClient(ILogger logger, IEventLogger eventLogger)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
        }

        public string FtpLocation { get; set; }

        public string FtpUserName { get; set; }

        public string FtpPassword { get; set; }

        public Stream GetResponseStream()
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(this.FtpLocation);

                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                
                request.Credentials = new NetworkCredential(this.FtpUserName, this.FtpPassword);

                var response = (FtpWebResponse)request.GetResponse();

                return response.GetResponseStream();
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to FTP to {this.FtpLocation} for {this.FtpUserName} {this.FtpPassword}", exception);
                this.eventLogger.TryWriteToEventLog(
                    EventSource.WellAdamXmlImport,
                    $"Error when trying to FTP to {this.FtpLocation} for {this.FtpUserName} - {this.FtpPassword}",
                    EventId.FtpException);

                return null;
            }
        }

        public void DeleteFile(string filename)
        {
            var request = (FtpWebRequest)WebRequest.Create(this.FtpLocation + "/" + filename);

            request.Method = WebRequestMethods.Ftp.DeleteFile;

            request.Credentials = new NetworkCredential(this.FtpUserName, this.FtpPassword);

            var response = (FtpWebResponse)request.GetResponse();

            if (response.StatusCode != FtpStatusCode.FileActionOK)
            {
                // TODO do something
            }
        }
    }
}