namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain;
    using Repositories.Contracts;
    using System.Linq;
    using Domain.Extensions;
    using Domain.Enums;

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

        public SingleLocation Get(int id)
        {
            var data = this.locationRepository.GetSingleLocationById(id);
            var jobIds = data.Details.Select(p => p.JobId).Distinct();
            var assignees = assigneeRepository.GetByJobIds(jobIds).ToDictionary(k => k.JobId);

            data.Details = data.Details
                .Select(p =>
                {
                    p.Assignee = assignees.ContainsKey(p.JobId) ? 
                        assignees[p.JobId] 
                        : new Domain.ValueObjects.Assignee { Name = "Unallocated" };
                    p.JobStatus = EnumExtensions.GetDescription((WellStatus)p.WellStatus);

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