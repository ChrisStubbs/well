namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Repositories.Contracts;
    using static System.String;


    public class AccountController : ApiController
    {
        private readonly ILogger logger;

        private readonly IAccountRepository accountRespository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        
        public AccountController(
            ILogger logger,
            IAccountRepository accountRepository, IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.logger = logger;
            this.accountRespository = accountRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [Route("account", Name = "GetAccountByStopId")]
        [HttpGet]
        public HttpResponseMessage GetAccountByStopId(int stopId)
        {
            try
            {
                var account = this.accountRespository.GetAccountByStopId(stopId);
                return account.Code == Empty
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, account);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting account");
                return this.serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

        [Route("account", Name = "GetAccountByAccountId")]
        [HttpGet]
        public HttpResponseMessage GetAccountByAccountId(int accountId)
        {
            try
            {
                var account = this.accountRespository.GetAccountByAccountId(accountId);
                return account.Code == Empty
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, account);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting account");
                return this.serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

    }
}