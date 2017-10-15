namespace PH.Well.Api
{
    using System.Configuration;
    using Services.Contracts;
    public class WellCleanConfig : IWellCleanConfig
    {
        public WellCleanConfig()
        {
            var x = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["CleanBatchSize"], out x))
            {
                CleanBatchSize = x;
            }

        }

        public int CleanBatchSize { get; set; } = 1000;
        public int WellCleanTransactionTimeoutSeconds { get; set; } = 1200;
    }
}