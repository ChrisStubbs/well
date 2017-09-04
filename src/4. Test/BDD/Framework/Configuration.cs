namespace PH.Well.BDD.Framework
{
    using System;
    using System.Configuration;

    public struct Configuration
    {
        public static string DatabaseConnection => ConfigurationManager.ConnectionStrings["Well"].ConnectionString;

        public static string Database => ConfigurationManager.AppSettings["Database"];

        public static string SqlInstance => ConfigurationManager.AppSettings["SqlInstance"];

        public static string PathToDacpac => ConfigurationManager.AppSettings["PathToDacpac"];

        public static int DriverTimeoutSeconds => int.Parse(ConfigurationManager.AppSettings["DriverTimeoutInSeconds"]);

        public static string WellApiUrl => ConfigurationManager.AppSettings["WellApiUrl"];
        public static string SecurityApiUrl => ConfigurationManager.AppSettings["SecurityApiUrl"];

        public static string DashboardUrl => ConfigurationManager.AppSettings["DashboardUrl"];

        public static int TransactionTimeout => int.Parse(ConfigurationManager.AppSettings["transactionTimeoutSeconds"]);
        public static string ApplicationId => ConfigurationManager.AppSettings["ApplicationId"];

        public static Driver Driver => (Driver)Enum.Parse(typeof(Driver), ConfigurationManager.AppSettings["WebDriver"]);

        public static bool UseWaitForAngular2 => bool.Parse(ConfigurationManager.AppSettings["UseWaitForAngular2"]);
    }
}