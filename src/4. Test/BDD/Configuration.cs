namespace PH.Well.BDD
{
    using System.Configuration;

    public struct Configuration
    {
        public static readonly string DatabaseConnection = ConfigurationManager.ConnectionStrings["OverriderDiscounts"].ConnectionString;

        public static readonly string Database = ConfigurationManager.AppSettings["Database"];

        public static readonly string SqlInstance = ConfigurationManager.AppSettings["SqlInstance"];

        public static readonly string PathToDacpac = ConfigurationManager.AppSettings["PathToDacpac"];

        public static readonly int TransactionTimeout = int.Parse(ConfigurationManager.AppSettings["TransactionTimeout"]);
    }
}
