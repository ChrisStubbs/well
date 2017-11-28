namespace PH.Well.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Repositories.Contracts;
    using Domain;

    public class AccountController : ApiController
    {
        private readonly IAccountRepository accountRespository;
        
        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRespository = accountRepository;
        }

        public Account Get(int id)
        {
            var account = this.accountRespository.GetAccountByAccountId(id);

            if (account == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            return account;
        }
    }
}