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
            
        }

        public int SoftDeleteBatchSize { get; set; } = 1000;
    }
}