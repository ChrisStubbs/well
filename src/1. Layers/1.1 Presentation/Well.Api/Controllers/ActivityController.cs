using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    using System;
    using Domain.ValueObjects;
    using Repositories;
    using Repositories.Contracts;

    public class ActivityController : ApiController
    {
        private readonly IActivityRepository activityRepository;

        public ActivityController(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        [HttpGet]
        [Route("Activity/{invoice}/{branchId:int}")]
        public ActivitySource Get(string invoice, int branchId)
        {
            //var invoiceNumber = invoice.ToString();
            return this.activityRepository.GetActivitySourceByDocumentNumber(invoice, branchId);

        }
    }
}
