namespace PH.Well.Adam.Infrastructure
{
    using Contracts;
    using System.Configuration;

    public class Configuration : IAdamImportConfiguration
    {
        public Configuration()
        {
            FilePath = ConfigurationManager.AppSettings["downloadFilePath"];
            SearchPattern = "*.xml*";
        }
        public string FilePath { get; set; }
        public string SearchPattern { get; set; }
    }
}
