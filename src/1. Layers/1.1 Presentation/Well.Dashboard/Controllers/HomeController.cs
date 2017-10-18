namespace PH.Well.Dashboard.Controllers
{
    using System.Web.Mvc;
    using Elmah;

    public class HomeController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public virtual ActionResult AppLayout()
        {
            return View();
        }
    }
}