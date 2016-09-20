namespace PH.Well.Api.Models
{
    public class GlobalSettingsModel
    {
        public string Version { get; set; }

        public string UserName { get; set; }

        public string IdentityName { get; set; }

        public string[] Permissions { get; set; }
    }
}