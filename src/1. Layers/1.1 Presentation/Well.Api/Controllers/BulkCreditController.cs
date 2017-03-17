namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Domain.ValueObjects;
    using Models;
    using Repositories.Contracts;
    using Services.Contracts;

    public class BulkCreditController : BaseApiController
    {
        private readonly IBulkCreditService bulkCreditService;
        private readonly IJobRepository jobRepository;


        public BulkCreditController(IBulkCreditService bulkCreditService,
            IUserNameProvider userNameProvider,
            IJobRepository jobRepository) 
            : base(userNameProvider)
        {
            this.bulkCreditService = bulkCreditService;
            this.jobRepository = jobRepository;
        }

        [HttpPost]
        [Route("bulk-credit/")]
        public HttpResponseMessage BulkCredit(BulkCreditModel model)
        {
            var jobs = jobRepository.GetByIds(model.JobIds);

            if (jobs.Count() != model.JobIds.Count)
            {
                return this.Request.CreateResponse(new { notAcceptable = true, message = new List<string>() {"Unable to find deliveries to bulk credit"} });
            }

            var warnings = bulkCreditService.BulkCredit(jobs, model.Reason, model.Source);
            if (warnings.Any())
            {
                return this.Request.CreateResponse(new { notAcceptable = true, message = warnings });
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}