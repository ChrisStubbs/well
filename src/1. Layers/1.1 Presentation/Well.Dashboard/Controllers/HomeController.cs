namespace PH.Well.Dashboard.Controllers
{
    using PH.Well.Common.Contracts;

    public class HomeController : BaseController
    {
        public HomeController(IWebClient webClient)
            : base(webClient)
        {
        }
    }
}