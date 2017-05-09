using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    using System.Linq;
    using Mapper.Contracts;
    using Models;
    using Repositories.Contracts;

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

            return null;
            //return new List<SingleRoute>
            //{
            //    new SingleRoute { Stop = "001", Invoice = "9875654321", JobType = "Frozen", JobStatus = "Clean", Cod = "1", Pod = true, Exceptions = 0, Clean = 10,  Tba = 0, Assignee = "Henrry Pires", Status = "Clean"},
            //    new SingleRoute { Stop = "001", Invoice = "324", JobType = "Chiled", JobStatus = "Exceptions", Cod = "1", Pod = true, Exceptions = 8, Clean = 10,  Tba = 0, Assignee = "Henrry Pires", Credit = 20.36M, Status = "Exception"},
            //    new SingleRoute { Stop = "002", Invoice = "56754", JobType = "Frozen", JobStatus = "Clean", Cod = "1", Pod = true, Exceptions = 0, Clean = 10,  Tba = 0, Assignee = "Henrry Pires", Status = "Resolve"},
            //    new SingleRoute { Stop = "003", Invoice = "56785", JobType = "Frozen", JobStatus = "Exceptions", Cod = "1", Pod = true, Exceptions = 4, Clean = 2,  Tba = 0, Assignee = "Henrry Pires", Credit = 139.88M, Status = "Exception"},
            //    new SingleRoute { Stop = "003", Invoice = "561785", JobType = "Ambient", JobStatus = "Exceptions", Cod = "1", Pod = true, Exceptions = 0, Clean = 10,  Tba = 1, Assignee = "Henrry Pires", Status = "Approval"}
            //};
        }

      
    }

  
}
