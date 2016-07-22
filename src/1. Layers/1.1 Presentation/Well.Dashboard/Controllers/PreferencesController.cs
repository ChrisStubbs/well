namespace PH.Well.Dashboard.Controllers
{
    using PH.Well.Common.Contracts;

    public class PreferencesController : BaseController
    {
        public PreferencesController(IWebClient webClient)
            : base(webClient)
        {
        }
    }
}