namespace PH.Well.Api.Controllers
{
    using System.Web.Http;

    using System.Linq;
    using Mapper.Contracts;
    using Models;
    using Repositories.Contracts;
    using System.Net.Http;
    using System.Net;

    public class SingleRouteController : ApiController
    {
        private readonly IBranchRepository branchRepository;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;
        private readonly IAssigneeReadRepository assigneeRepository;
        private readonly ISingleRouteMapper mapper;

        public SingleRouteController(
            IBranchRepository branchRepository,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IAssigneeReadRepository assigneeRepository,
            ISingleRouteMapper mapper)
        {
            this.branchRepository = branchRepository;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.assigneeRepository = assigneeRepository;
            this.mapper = mapper;
        }

        public SingleRoute Get(int id)
        {
            var routeHeader = routeHeaderRepository.GetRouteHeaderById(id);

            if (routeHeader != null)
            {
                var branches = branchRepository.GetAll().ToList();
                var stops = stopRepository.GetStopByRouteHeaderId(id).ToList();
                var jobs = jobRepository.GetByRouteHeaderId(id).ToList();
                var assignees = assigneeRepository.GetByRouteHeaderId(id).ToList();

                return mapper.Map(branches, routeHeader, stops, jobs, assignees);
            }

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
