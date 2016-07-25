namespace PH.Well.Dashboard.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Web.Mvc;

    using Newtonsoft.Json;

    using PH.Well.Common.Contracts;
    using PH.Well.Dashboard.Models;

    public abstract class BaseController : Controller
    {
        public string UserName => Thread.CurrentPrincipal.Identity.Name;

        protected BootstrapData Model { get; set; }

        protected BaseController(IWebClient webClient)
        {
            var config = new Dictionary<string, string>
            {
                {"apiUrl", Configuration.OrderWellApi}
            };

            var version = JsonConvert.DeserializeObject<ApiVersion>(webClient.DownloadString(Configuration.OrderWellApi + "version"));

            var userBranches = webClient.DownloadString(Configuration.OrderWellApi + "user-branches");

            Model = new BootstrapData
            {
                Version = version.Version,
                UsersBranches = userBranches.Trim('"'),
                ConfigDictionary = config
            };
        }

        [HttpGet]
        public ActionResult Index()
        {
           
            return this.View("Index", Model);
        }
    }
}