namespace PH.Well.Clean
{
    using System.Configuration;

    public struct Configuration
    {
        public static string WellConnection => ConfigurationManager.ConnectionStrings["Well"].ConnectionString;
    }
}
 