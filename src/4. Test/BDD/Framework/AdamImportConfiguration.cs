namespace PH.Well.BDD.Framework
{
    using System;
    using System.IO;
    using Adam.Contracts;

    public class AdamImportConfiguration : IAdamImportConfiguration
    {
        public AdamImportConfiguration()
        {
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xml");
            SearchPattern = "PH_ROUTES*.xml";
        }
        public string FilePath { get; set; }
        public string SearchPattern { get; set; }
    }
}
