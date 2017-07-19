using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class ActivityController : ApiController
    {
        private readonly IActivityRepository activityRepository;

        public ActivityController(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }
        
        [Route("Activity/{invoice}/{branchId:int}")]
        public ActivitySource Get(string invoice, int branchId)
        {
            return this.activityRepository.GetActivitySourceByDocumentNumber(invoice, branchId);
        }
    }
}
