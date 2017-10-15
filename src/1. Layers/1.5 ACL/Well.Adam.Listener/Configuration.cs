namespace PH.Well.Adam.Listener
{
    using System.Configuration;
    using System.Data;
    using Common.Contracts;
    using Services.Contracts;

    public class Configuration : IDeadlockRetryConfig, IWellCleanConfig
    {

        public Configuration()
        {
            var x = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["CleanBatchSize"], out x))
            {
                CleanBatchSize = x;
            }

            x = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["WellCleanTransactionTimeoutSeconds"], out x))
            {
                WellCleanTransactionTimeoutSeconds = x;
            }

        }
        public static string RootFolder => ConfigurationManager.AppSettings["rootFolder"];
        public int MaxNoOfDeadlockRetires => int.Parse(ConfigurationManager.AppSettings["maxNoOfDeadlockRetries"]);
        public int DeadlockRetryDelayMilliseconds => int.Parse(ConfigurationManager.AppSettings["deadlockRetryDelayMilliseconds"]);

        public int CleanBatchSize { get; set; } = 1000;
        public int WellCleanTransactionTimeoutSeconds { get; set; } = 600;
    }
}
