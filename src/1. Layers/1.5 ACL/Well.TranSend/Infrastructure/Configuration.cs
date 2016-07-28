namespace PH.Well.TranSend.Infrastructure
{
    using System.Configuration;
    using Contracts;

    public class Configuration : IEpodImportConfiguration
    {

        public Configuration()
        {
            FilePath = ConfigurationManager.AppSettings["downloadFilePath"];
            ArchiveLocation = ConfigurationManager.AppSettings["archiveLocation"];
            SearchPattern = "*.xml*";
            FtpLocation = ConfigurationManager.AppSettings["transendFTPLocation"];
            FtpUser = ConfigurationManager.AppSettings["transendUser"];
            FtpPass = ConfigurationManager.AppSettings["transendPass"];
            NetworkUser = ConfigurationManager.AppSettings["localUser"];
            NetworkUserPass = ConfigurationManager.AppSettings["localUserPass"];
        }

        public string FilePath { get; set; }
        public string SearchPattern { get; set; }
        public string ArchiveLocation { get; set; }
        public string FtpLocation { get; set; }
        public string FtpUser { get; set; }
        public string FtpPass{ get; set; }
        public string NetworkUser { get; set; }
        public string NetworkUserPass { get; set; }
    }
}
