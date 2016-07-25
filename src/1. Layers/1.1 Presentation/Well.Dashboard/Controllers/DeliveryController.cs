namespace PH.Well.Dashboard.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Models;
    using Newtonsoft.Json;

    public class DeliveryController : Controller
    {

        protected BootstrapData Model { get; set; }
        
        [HttpGet]
        //[Route("delivery/{id:int}", Name = "GetDelivery")]
        public ActionResult Index()
        {
            var config = new Dictionary<string, string>
            {
                {"apiUrl", Configuration.OrderWellApi},
                {"deliveryId","4"} //id.ToString()}
            };

            Model = new BootstrapData
            {
                Configuration = JsonConvert.SerializeObject(config)
            };

            return this.View("Index", Model);
        }
    }
}