namespace PH.Well.FileDistributor.Infrastructure
{
    using System.Configuration;
    using Common.Contracts;

    public class Configuration : IDeadlockRetryConfig
    { 

        public static string WellConnection => ConfigurationManager.ConnectionStrings["Well"].ConnectionString;

        public static string FilePath => ConfigurationManager.AppSettings["downloadFilePath"];

        public static string SearchPattern => "*.xml*";

        public static string ArchiveLocation => ConfigurationManager.AppSettings["archiveLocation"];

        public static string FtpLocation => ConfigurationManager.AppSettings["FileDistributorFTPLocation"];

        public static string FtpUsername => ConfigurationManager.AppSettings["FileDistributorUser"];

        public static string FtpPassword => ConfigurationManager.AppSettings["FileDistributorPass"];

        public static string DashboardRefreshEndpoint => ConfigurationManager.AppSettings["dashboardRefreshEndpoint"];

        public static string DownloadFilePath => ConfigurationManager.AppSettings["downloadFilePath"];

        public int MaxNoOfDeadlockRetires => int.Parse(ConfigurationManager.AppSettings["maxNoOfDeadlockRetries"]);

        public int DeadlockRetryDelayMilliseconds => int.Parse(ConfigurationManager.AppSettings["deadlockRetryDelayMilliseconds"]);

        public static bool DeleteFtpFileAfterImport => bool.Parse(ConfigurationManager.AppSettings["deleteFtpFileAfterImport"]);

    }
}
