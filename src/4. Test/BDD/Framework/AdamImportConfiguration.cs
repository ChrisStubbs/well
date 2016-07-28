namespace PH.Well.BDD.Framework
{
    using System;
    using System.Configuration;
    using System.IO;
    using Adam.Contracts;

    public class AdamImportConfiguration : IAdamImportConfiguration
    {
        public AdamImportConfiguration()
        {
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xml");
            SearchPattern = "PH_ROUTES*.xml";
            ArchiveLocation = ConfigurationManager.AppSettings["archiveLocation"];
        }
        public string FilePath { get; set; }
        public string SearchPattern { get; set; }
        public string ArchiveLocation { get; set; }
    }
}
