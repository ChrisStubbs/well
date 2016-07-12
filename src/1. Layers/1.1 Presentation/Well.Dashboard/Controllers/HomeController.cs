namespace Well.Dashboard.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using PH.Well.Dashboard.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var config = new Dictionary<string, string>
            {
                {"apiUrl", ConfigurationManager.AppSettings["OrderWellApi"]}
            };

            var model = new BootstrapData
            {
                Configuration = JsonConvert.SerializeObject(config)
            };

            return this.View("Index",model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}