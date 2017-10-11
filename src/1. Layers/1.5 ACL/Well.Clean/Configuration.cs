namespace PH.Well.Clean
{
    using System.Configuration;
    using Services.Contracts;
    public class Configuration : IWellCleanConfig
    {
        public Configuration()
        {
            var x = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["SoftDeleteBatchSize"], out x))
            {
                SoftDeleteBatchSize = x;
            }

            x = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["WellCleanTransactionTimeoutSeconds"], out x))
            {
                WellCleanTransactionTimeoutSeconds = x;
            }

        }

        public int SoftDeleteBatchSize { get; set; } = 1000;
        public int WellCleanTransactionTimeoutSeconds { get; set; } = 600;
    }
}