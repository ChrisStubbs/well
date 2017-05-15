namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain;
    using Repositories.Contracts;

    public class ActivityReadController : ApiController
    {
        private readonly IActivityReadRepository activityReadRepository;

        public ActivityReadController(IActivityReadRepository activityReadRepository)
        {
            this.activityReadRepository = activityReadRepository;
        }

        public Activity GetById(int id)
        {
            return this.activityReadRepository.GetById(id);
        }
    }
}