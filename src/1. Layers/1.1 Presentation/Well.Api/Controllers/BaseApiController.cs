using PH.Well.Common.Contracts;

namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    
    public class BaseApiController : ApiController
    {
        protected IUserNameProvider UserNameProvider { get; }

        public string UserIdentityName => this.UserNameProvider.GetUserName();

        public BaseApiController(IUserNameProvider userNameProvider)
        {
            UserNameProvider = userNameProvider;
        }
    }
}
