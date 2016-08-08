namespace PH.Well.Adam.Events
{
    using System.Configuration;

    public struct Configuration
    {
        public static string WellApi => ConfigurationManager.AppSettings["OrderWellApi"];

        public static string AdamUsername => ConfigurationManager.AppSettings["adamUserName"];

        public static string AdamPassword => ConfigurationManager.AppSettings["adamPassword"];

        public static string AdamServer => ConfigurationManager.AppSettings["adamServer"];

        public static string AdamRfsMedway => ConfigurationManager.AppSettings["adamRfsMedway"];

        public static int AdamPortMedway => int.Parse(ConfigurationManager.AppSettings["adamPortMedway"]);

        public static string AdamRfsCoventry => ConfigurationManager.AppSettings["adamRfsCoventry"];

        public static int AdamPortCoventry => int.Parse(ConfigurationManager.AppSettings["adamPortCoventry"]);

        public static string AdamRfsFareham => ConfigurationManager.AppSettings["adamRfsFareham"];

        public static int AdamPortFareham => int.Parse(ConfigurationManager.AppSettings["adamPortFareham"]);

        public static string AdamRfsDunfermline => ConfigurationManager.AppSettings["adamRfsDunfermline"];

        public static int AdamPortDunfermline => int.Parse(ConfigurationManager.AppSettings["adamPortDunfermline"]);

        public static string AdamRfsLeeds => ConfigurationManager.AppSettings["adamRfsLeeds"];

        public static int AdamPortLeeds => int.Parse(ConfigurationManager.AppSettings["adamPortLeeds"]);

        public static string AdamRfsHemel => ConfigurationManager.AppSettings["adamRfsHemel"];

        public static int AdamPortHemel => int.Parse(ConfigurationManager.AppSettings["adamPortHemel"]);

        public static string AdamRfsBirtley => ConfigurationManager.AppSettings["adamRfsBirtley"];

        public static int AdamPortBirtley => int.Parse(ConfigurationManager.AppSettings["adamPortBirtley"]);

        public static string AdamRfsBelfast => ConfigurationManager.AppSettings["adamRfsBelfast"];

        public static int AdamPortBelfast => int.Parse(ConfigurationManager.AppSettings["adamPortBelfast"]);

        public static string AdamRfsBrandon => ConfigurationManager.AppSettings["adamRfsBrandon"];

        public static int AdamPortBrandon => int.Parse(ConfigurationManager.AppSettings["adamPortBrandon"]);

        public static string AdamRfsPlymouth => ConfigurationManager.AppSettings["adamRfsPlymouth"];

        public static int AdamPortPlymouth => int.Parse(ConfigurationManager.AppSettings["adamPortPlymouth"]);

        public static string AdamRfsBristol => ConfigurationManager.AppSettings["adamRfsBristol"];

        public static int AdamPortBristol => int.Parse(ConfigurationManager.AppSettings["adamPortBristol"]);

        public static string AdamRfsHaydock => ConfigurationManager.AppSettings["adamRfsHaydock"];

        public static int AdamPortHaydock => int.Parse(ConfigurationManager.AppSettings["adamPortHaydock"]);
    }
}