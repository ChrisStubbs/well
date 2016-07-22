namespace PH.Well.Api.Controllers
{
    using System.Threading;
    using System.Web.Http;

    public class BaseApiController : ApiController
    {
        public string UserName => Thread.CurrentPrincipal.Identity.Name;
    }
}