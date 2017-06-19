namespace PH.Well.Api.Controllers
{
    using System.Linq;
    using Mapper.Contracts;
    using Models;
    using Repositories.Contracts;
    using System.Web.Http;

    public class StopsController : ApiController
    {
        private readonly IBranchRepository branchRepository;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;
        private readonly IAssigneeReadRepository assigneeRepository;
        private readonly IStopMapper stopMapper;

        public StopsController(
            IBranchRepository branchRepository,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IAssigneeReadRepository assigneeRepository,
            IStopMapper stopMapper)
        {
            this.branchRepository = branchRepository;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.assigneeRepository = assigneeRepository;
            this.stopMapper = stopMapper;
        }

        public StopModel Get(int id)
        {
            var stop = stopRepository.GetById(id);
            if (stop != null)
            {
                return stopMapper.Map(
                    branchRepository.GetAll().ToList(),
                    routeHeaderRepository.GetRouteHeaderById(stop.RouteHeaderId), 
                    stop,
                    jobRepository.GetByStopId(stop.Id).ToList(),
                    assigneeRepository.GetByStopId(stop.Id).ToList(),
                    jobRepository.JobDetailTotalsPerStop(stop.Id));
            }

            return null;
        }
    }
}
