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
                    var item = new SingleRouteItem();
                    item.JobId = job.Id;
                    item.StopId = job.StopId;
                    item.Stop = stop.DropId;
                    item.StopStatus = stopStatusService.DetermineStatus(stopJobs);
                    item.StopExceptions = stopJobDetails.Count(x => !x.IsClean());
                    item.StopClean = stopJobDetails.Count(x => x.IsClean());
                    item.Tba = stopJobs.Sum(j => j.ToBeAdvisedCount);
                    item.StopAssignee = Assignee.GetDisplayNames(assignee.Where(x => x.StopId == stop.Id).ToList());
                    item.Resolution = "TODO:";
                    item.Invoice = job.InvoiceNumber;
                    item.JobType = EnumExtensions.GetValueFromDescription<JobType>(job.JobTypeCode).ToString().SplitCapitalisedWords();
                    item.JobStatus = job.JobStatus;
                    item.JobStatusDescription = jobStatuses[job.JobStatus];
                    item.Cod = job.Cod;
                    item.Pod = job.ProofOfDelivery.HasValue;
                    item.Exceptions = job.JobDetails.Count(x => !x.IsClean());
                    item.Clean = job.JobDetails.Count(x => x.IsClean());
                    item.Credit = 0;  //TODO: Needs to come from Line Item Action
                    item.Assignee = Assignee.GetDisplayNames(assignee.Where(x => x.JobId == job.Id).ToList());
                    singleRoute.Items.Add(item);
                }
            }
            return singleRoute;
        }
    }
}