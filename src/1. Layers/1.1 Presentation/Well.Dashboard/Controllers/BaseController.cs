namespace PH.Well.Dashboard.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Newtonsoft.Json;

    using PH.Well.Dashboard.Models;

    public abstract class BaseController : Controller
    {
        protected BootstrapData Model { get; set; }

        protected BaseController()
        {
            var config = new Dictionary<string, string>
            {
                {"apiUrl", Configuration.OrderWellApi}
            };

            Model = new BootstrapData
            {
                Configuration = JsonConvert.SerializeObject(config),
            };
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View("Index", Model);
        }
    }
}