namespace PH.Well.Api.Controllers
{
    using System.Threading;
    using System.Web.Http;

    public class BaseApiController : ApiController
    {
        public string UserIdentityName => Thread.CurrentPrincipal.Identity.Name;
    }
}