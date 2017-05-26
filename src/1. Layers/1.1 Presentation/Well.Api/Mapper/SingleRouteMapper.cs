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
                var stopJobs = jobs.Where(x => x.StopId == stop.Id).ToList();

                var stopJobDetails = stopJobs.SelectMany(x => x.JobDetails).ToArray();

                foreach (var job in stopJobs)
                {
                    var item = new SingleRouteItem
                    {
                        JobId = job.Id,
                        StopId = job.StopId,
                        Stop = stop.DropId,
                        StopStatus = stopStatusService.DetermineStatus(stopJobs),
                        StopExceptions = stopJobDetails.Count(x => !x.IsClean()),
                        StopClean = stopJobDetails.Count(x => x.IsClean()),
                        Tba = stopJobs.Sum(j => j.ToBeAdvisedCount),
                        StopAssignee = Assignee.GetDisplayNames(assignee.Where(x => x.StopId == stop.Id).ToList()),
                        Resolution = "TODO:", //TODO: we have to fix this
                        Invoice = job.InvoiceNumber,
                        JobType = EnumExtensions.GetValueFromDescription<JobType>(job.JobTypeCode).ToString().SplitCapitalisedWords(),
                        JobStatus = job.JobStatus,
                        JobStatusDescription = jobStatuses[job.JobStatus],
                        Cod = job.Cod,
                        Pod = job.ProofOfDelivery.HasValue,
                        Exceptions = job.JobDetails.Count(x => !x.IsClean()),
                        Clean = job.JobDetails.Count(x => x.IsClean()),
                        Credit = 0,  //TODO: Needs to come from Line Item Action
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