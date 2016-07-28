namespace PH.Well.Api
{
    using System.Configuration;

    public struct Configuration
    {
        public static string DomainsToSearch => ConfigurationManager.AppSettings["Domains"];
    }
}
