namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Repositories.Contracts;

    public class AccountController : ApiController
    {
        private readonly IAccountRepository accountRespository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        
        public AccountController(IAccountRepository accountRepository, IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.accountRespository = accountRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [Route("account/{accountId:int}", Name = "GetAccountByAccountId")]
        [HttpGet]
        public HttpResponseMessage GetAccountByAccountId(int accountId)
        {
            try
            {
                var account = this.accountRespository.GetAccountByAccountId(accountId);
                return string.IsNullOrWhiteSpace(account.Code)
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, account);
            }
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex, $"An error occurred when getting account by id {accountId}");
            }
        }
    }
}