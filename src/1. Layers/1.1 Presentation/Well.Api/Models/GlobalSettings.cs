using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PH.Well.Api.Models
{
    public class GlobalSettingsModel
    {
        public string Version { get; set; }
        public string UserName { get; set; }
        public string IdentityName { get; set; }
    }
}