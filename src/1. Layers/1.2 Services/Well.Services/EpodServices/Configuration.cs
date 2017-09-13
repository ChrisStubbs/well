namespace PH.Well.Services.EpodServices
{
    using System.Configuration;
    using Common.Contracts;

    public class Configuration : IDeadlockRetryConfig
    { 

        public static string WellConnection => ConfigurationManager.ConnectionStrings["Well"].ConnectionString;

        public static string FilePath => ConfigurationManager.AppSettings["downloadFilePath"];

        public static string SearchPattern => "*.xml*";

        public static string ArchiveLocation => ConfigurationManager.AppSettings["archiveLocation"];

        public static string FtpLocation => ConfigurationManager.AppSettings["transendFTPLocation"];

        public static string FtpUsername => ConfigurationManager.AppSettings["transendUser"];

        public static string FtpPassword => ConfigurationManager.AppSettings["transendPass"];

        public static string DashboardRefreshEndpoint => ConfigurationManager.AppSettings["dashboardRefreshEndpoint"];

        public static string DownloadFilePath => ConfigurationManager.AppSettings["downloadFilePath"];

        public int MaxNoOfDeadlockRetires => int.Parse(ConfigurationManager.AppSettings["maxNoOfDeadlockRetries"]);

        public int DeadlockRetryDelayMilliseconds => int.Parse(ConfigurationManager.AppSettings["deadlockRetryDelayMilliseconds"]);
    }
}
