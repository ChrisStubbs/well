namespace PH.Well.Adam.Listener
{
    using System.Configuration;

    public struct Configuration
    {
        public static string RootFolder => ConfigurationManager.AppSettings["rootFolder"];

        
    }
}
