using System.Configuration;

namespace PH.Well.TranSend
{
    public struct Configuration
    {
        public static string FileLocation => ConfigurationManager.AppSettings["transcendFileUpdatesLocation"] ?? "Not known";
    }
}