namespace PH.Well.Adam.Listener
{
    using System.Configuration;
    using System.Data;
    using Common.Contracts;

    public class Configuration : IDeadlockRetryConfig
    {
        public static string RootFolder => ConfigurationManager.AppSettings["rootFolder"];
        public int MaxNoOfDeadlockRetires => int.Parse(ConfigurationManager.AppSettings["maxNoOfDeadlockRetries"]);
        public int DeadlockRetryDelayMilliseconds => int.Parse(ConfigurationManager.AppSettings["deadlockRetryDelayMilliseconds"]);
    }
}
