namespace PH.Well.Dashboard.Controllers
{
    using PH.Well.Common.Contracts;

    public class CleanController : BaseController
    {
        public CleanController(IWebClient webClient)
            : base(webClient)
        {
        }
    }
}