namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Models;
    using Repositories.Contracts;

    public class AuditController : ApiController
    {
        private readonly IAuditRepository auditRepository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        
        public AuditController(IAuditRepository auditRepository, IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.auditRepository = auditRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [Route("audits")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                var audits = auditRepository.Get();

                if (audits == null || audits.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, audits.Select(a => new AuditModel()
                {
                    Entry = a.Entry,
                    AccountName = a.AccountName,
                    AccountCode = a.AccountCode,
                    AuditBy = a.CreatedBy,
                    AuditDate = a.DateCreated,
                    DeliveryDate = a.DeliveryDate.Value,
                    InvoiceNumber = a.InvoiceNumber,
                    Type = a.Type.ToString()
                }));
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, $"An error occured when getting audits");
            }
        }
    }
}