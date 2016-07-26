namespace PH.Well.Dashboard.Controllers
{
    using PH.Well.Common.Contracts;

    public class NotificationsController : BaseController
    {
        public NotificationsController(IWebClient webClient)
            : base(webClient)
        {
        }
    }
}