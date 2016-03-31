namespace PH.Well.BDD
{
    using System.Configuration;

    public struct Configuration
    {
        public static string DatabaseConnection = ConfigurationManager.ConnectionStrings["OverriderDiscounts"].ConnectionString;

        public static string Database = ConfigurationManager.AppSettings["Database"];

        public static string SqlInstance = ConfigurationManager.AppSettings["SqlInstance"];

        public static string PathToDacpac = ConfigurationManager.AppSettings["PathToDacpac"];

        public static int TransactionTimeout = int.Parse(ConfigurationManager.AppSettings["TransactionTimeout"]);
    }
}
