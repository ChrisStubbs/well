namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;
    using Domain;
    using Domain.ValueObjects;
    using Contracts;
    using Domain.Enums;
    using Domain.Extensions;
    using Models;
    using Services.Contracts;
    using Branch = Domain.Branch;

    public class SingleRouteMapper : ISingleRouteMapper
    {
        private readonly IStopStatusService stopStatusService;
        private readonly IDictionary<JobStatus, string> jobStatuses = Enum<JobStatus>.GetValuesAndDescriptions();

        public SingleRouteMapper(IStopStatusService stopStatusService)
        {
            this.stopStatusService = stopStatusService;
        }
        public SingleRoute Map(List<Branch> branches, RouteHeader route, List<Stop> stops, List<Job> jobs, List<Assignee> assignee)
        {
            var singleRoute = new SingleRoute
            {
                Id = route.Id,
                RouteNumber = route.RouteNumber,
                Branch = branches.Single(x => x.Id == route.RouteOwnerId).BranchName,
                BranchId = route.RouteOwnerId,
                Driver = route.DriverName,
                RouteDate = route.RouteDate
            };

            return MapItems(singleRoute, stops, jobs, assignee);
        }

        private SingleRoute MapItems(SingleRoute singleRoute, List<Stop> stops, List<Job> jobs, List<Assignee> assignee)
        {

            foreach (var stop in stops)
            {
                var stopJobs = jobs
                    .Select(p => new
                    {
                        obj = p,
                        jobType = EnumExtensions.GetValueFromDescription<JobType>(p.JobTypeCode)
                    })
                    .Where(p => p.jobType != JobType.Documents && p.obj.StopId == stop.Id)
                    .Select(p => p.obj) //I should not do this but it's to much code to change with very little gain
                    .ToList();

                var stopJobDetails = stopJobs.SelectMany(x => x.JobDetails).ToArray();
                var tba = stopJobs.Sum(j => j.ToBeAdvisedCount);

                var clean = stopJobDetails.GroupBy(p => p.IsClean())
                    .Select(p => new { p.Key, count = p.Count() })
                    .ToLookup(k => k.Key, v => v.count);

                var stopExceptions = clean[false].Sum(p => p);
                var stopClean = clean[true].Sum(p => p);
                var status = EnumExtensions.GetDescription((WellStatus)stop.WellStatusId);
                var stopAssignee = Assignee.GetDisplayNames(assignee.Where(x => x.StopId == stop.Id).ToList());

                foreach (var job in stopJobs)
                {
                    var item = new SingleRouteItem
                    {
                        JobId = job.Id,
                        StopId = job.StopId,
                        Stop = stop.DropId,
                        StopStatus = status,
                        StopExceptions = stopExceptions,
                        StopClean = stopClean,
                        Tba = tba,
                        StopAssignee = stopAssignee,
                        Resolution = "TODO:", //TODO: we have to fix this
                        Invoice = job.InvoiceNumber,
                        JobType = EnumExtensions.GetValueFromDescription<JobType>(job.JobTypeCode).ToString().SplitCapitalisedWords(),
                        JobStatus = job.JobStatus,
                        JobStatusDescription = jobStatuses[job.JobStatus],
                        Cod = job.Cod,
                        Pod = job.ProofOfDelivery.HasValue,
                        Exceptions = job.JobDetails.Count(x => !x.IsClean()),
                        Clean = job.JobDetails.Count(x => x.IsClean()),
                        Credit = job.CreditValue,
                        Assignee = Assignee.GetDisplayNames(assignee.Where(x => x.JobId == job.Id).ToList()),
                        Account = job.PhAccount
                    };

                    singleRoute.Items.Add(item);
                }
            }
            return singleRoute;
        }
    }
}