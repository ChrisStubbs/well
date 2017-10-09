namespace PH.Well.Dashboard.Controllers
{
    using System.Web.Mvc;
    using Elmah;

    public class HomeController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index()
        {
            //Elmah.ErrorSignal.FromCurrentContext().Raise()
            var customEx = new System.Exception("Hello I am testing Elmah", new System.NotSupportedException());
            ErrorSignal.FromCurrentContext().Raise(customEx);


            return View();
        }

        [HttpGet]
        public virtual ActionResult AppLayout()
        {
            return View();
        }
    }
}