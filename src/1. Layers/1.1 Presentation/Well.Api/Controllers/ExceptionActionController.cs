namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Common.Extensions;
    using Domain.Enums;

    public class ExceptionActionController : BaseApiController
    {
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public ExceptionActionController(IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [HttpGet]
        [Route("exception-actions")]
        public HttpResponseMessage Get()
        {
            try
            {
                IEnumerable<ExceptionAction> actions = Enum.GetValues(typeof(ExceptionAction)).Cast<ExceptionAction>();
                var reasons = actions.Select(a => new {id = (int) a, description = StringExtensions.GetEnumDescription(a)});
                return Request.CreateResponse(HttpStatusCode.OK, reasons);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occcured when getting exception actions");
            }
        }
    }
}