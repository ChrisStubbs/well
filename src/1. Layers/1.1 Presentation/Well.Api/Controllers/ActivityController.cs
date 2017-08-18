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
        
        public ActivitySource Get(int id)
        {
            return this.activityRepository.GetActivitySourceById(id);
        }
    }
}
