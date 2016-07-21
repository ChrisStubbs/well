namespace PH.Well.Dashboard
{
    using System.Configuration;

    public struct Configuration
    {
        public static string OrderWellApi => ConfigurationManager.AppSettings["OrderWellApi"];

        public static string ApplicationName => ConfigurationManager.AppSettings["ApplicationName"];

        public static string SecurityApi => ConfigurationManager.AppSettings["SecurityApi"];
    }
}