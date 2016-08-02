﻿namespace PH.Well.Dashboard.Controllers
{
    using global::System.Web.Mvc;
    using Common.Contracts;

    public class DeliveryController : BaseController
    {
        public DeliveryController()
        //public DeliveryController(IWebClient webClient) : base(webClient)
        {
        }

        public override ActionResult Index()
        {
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        [Route("exceptions/delivery/{id:int}", Name = "GetExceptionDelivery")]
        [Route("clean/delivery/{id:int}", Name = "GetCleanDelivery")]
        [Route("resolved/delivery/{id:int}", Name = "GetResolvedDelivery")]
        [Route("delivery/{id:int}", Name = "GetDelivery")]
        public ActionResult Index(int id)
        {
            Model.ConfigDictionary.Add("deliveryId",id.ToString());
            return this.View("Index", Model);
        }

    }
}