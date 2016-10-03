namespace PH.Well.Clean.Infrastructure
{
    using System.Configuration;

    public struct Configuration
    {
        public static string WellConnection => ConfigurationManager.ConnectionStrings["Well"].ConnectionString;
    }
}
 