namespace PH.Well.Dashboard.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class BootstrapData
    {
        public BootstrapData()
        {
            ConfigDictionary = new Dictionary<string, string>();
        }

        public string UserName { get; set; }

        public string Configuration => JsonConvert.SerializeObject(ConfigDictionary);

        public string Version { get; set; }

        public string UsersBranches { get; set; }

        public Dictionary<string, string> ConfigDictionary { get; set; }
    }
}