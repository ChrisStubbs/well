namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain;
    using Repositories.Contracts;
    using System.Linq;

    public class SingleLocationController : ApiController
    {
        private readonly ILocationRepository locationRepository;
        private readonly IAssigneeReadRepository assigneeRepository;

        public SingleLocationController(ILocationRepository locationRepository,
            IAssigneeReadRepository assigneeRepository)
        {
            this.locationRepository = locationRepository;
            this.assigneeRepository = assigneeRepository;
        }

        public SingleLocation Get([FromUri]SingleLocationQuery qs)
        {
            var data = this.locationRepository.GetSingleLocation(qs.LocationId, qs.AccountNumber, qs.BranchId);
            var jobIds = data.Details.Select(p => p.JobId).Distinct();
            var assignees = assigneeRepository.GetByJobIds(jobIds).ToDictionary(k => k.JobId, v => v.Name);

            data.Details = data.Details
                .Select(p =>
                {
                    p.Assignee = assignees.ContainsKey(p.JobId) ? assignees[p.JobId] : string.Empty;

                    return p;
                })
                .ToList();

            return data;
        }
    }

    public class SingleLocationQuery
    {
        public int? LocationId { get; set; }
        public string AccountNumber { get; set; }
        public int? BranchId { get; set; }
    }
}