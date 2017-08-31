using PH.Well.Common.Contracts;

namespace PH.Well.Api.Controllers
{
    using System.Threading;
    using System.Web.Http;
    
    public class BaseApiController : ApiController
    {
        protected IUserNameProvider UserNameProvider { get; }

        ////// public string UserIdentityName => Thread.CurrentPrincipal.Identity.Name;
        public string UserIdentityName => this.UserNameProvider.GetUserName();

        public BaseApiController(IUserNameProvider userNameProvider)
        {
            UserNameProvider = userNameProvider;
        }
    }
}
