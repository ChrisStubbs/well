namespace PH.Well.Api.Mapper
{
    using System;
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

        public SingleRoute Map(List<Branch> branches,
            RouteHeader route,
            List<Stop> stops,
            List<Job> jobs,
            List<Assignee> assignee,
            IList<JobDetailLineItemTotals> jobDetailTotalsPerRouteHeader,
            Dictionary<int, string> jobPrimaryAccountNumber)
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

            return MapItems(singleRoute, stops, jobs, assignee, jobDetailTotalsPerRouteHeader, jobPrimaryAccountNumber);
        }

        private SingleRoute MapItems(
            SingleRoute singleRoute,
            List<Stop> stops,
            List<Job> jobs,
            List<Assignee> assignee,
            IList<JobDetailLineItemTotals> jobDetailTotalsPerRouteHeader,
            Dictionary<int, string> jobPrimaryAccountNumber)
        {

            foreach (var stop in stops)
            {
                var stopJobs = jobs
                    .Where(j => j.JobTypeEnumValue != JobType.Documents && j.StopId == stop.Id)
                    .ToList();
                
              //  var tba = stopJobs.Sum(j => j.ToBeAdvisedCount);  //todo don't think this is right
                // jobs may be grouped together for delivery, indicated by the OuterCount (ie all jobs
                // counted together for a stop have the same OuterCount)
                // All the jobs grouped together should have the same to be advised count 
                // This is the shorts to be advised total for the GROUP
                // There may be more than one group per stop
                var jobGroupToBeAdvised = stopJobs.GroupBy(j => new {j.OuterCount, j.ToBeAdvisedCount})
                    .Select(
                        y =>
                            new ToBeAdvisedGroup()
                            {
                                OuterCountId = y.Key.OuterCount.GetValueOrDefault(),
                                ToBeAdvisedCount = y.Key.ToBeAdvisedCount
                            }).ToList();

                var status = EnumExtensions.GetDescription(stop.WellStatus);
                var stopAssignee = Assignee.GetDisplayNames(assignee.Where(x => x.StopId == stop.Id).ToList());

                var jobExceptions = jobDetailTotalsPerRouteHeader
                    .ToLookup(k => k.JobId);

                foreach (var job in stopJobs)
                {
                    JobType jobType = EnumExtensions.GetValueFromDescription<JobType>(job.JobTypeCode);
                    var totalException = jobExceptions[job.Id].Sum(p => p.TotalExceptions);
                    var clean = job.JobDetails
                        .Where(x => x.IsClean() && !x.IsTobaccoBag())
                        .Sum(p => p.OriginalDespatchQty) - totalException;

                    var item = new SingleRouteItem
                    {
                        JobId = job.Id,
                        StopId = job.StopId,
                        Stop = stop.DropId,
                        StopStatus = status,
                        Previously = stop.Previously,
                        Tba = jobGroupToBeAdvised
                            .Where(x => x.OuterCountId == job.OuterCount)
                            .Select(y => y.ToBeAdvisedCount).FirstOrDefault(),
                        StopAssignee = stopAssignee,
                        Resolution = job.ResolutionStatus.Description,
                        ResolutionId = job.ResolutionStatus.Value,
                        Invoice = job.InvoiceNumber,
                        InvoiceId = job.ActivityId,
                        JobType = $"{jobType.ToString().SplitCapitalisedWords()} ({job.JobTypeAbbreviation})",
                        JobTypeId = (int)jobType,
                        JobStatus = job.JobStatus,
                        JobStatusDescription = jobStatuses[job.JobStatus],
                        Cod = job.Cod,
                        Pod = job.IsProofOfDelivery,
                        Exceptions = totalException,
                        Clean = clean,
                        Credit = job.CreditValue,
                        Assignee = Assignee.GetDisplayNames(assignee.Where(x => x.JobId == job.Id).ToList()),
                        Account = job.PhAccount,
                        AccountName = job.PhAccountName,
                        WellStatus = job.WellStatus,
                        WellStatusDescription = EnumExtensions.GetDescription(job.WellStatus),
                        GrnProcessType =  job.GrnProcessType ?? 0,
                        GrnNumber =  job.GrnNumber,
                        PrimaryAccountNumber = jobPrimaryAccountNumber[job.Id],
                        LocationId = stop.LocationId,
                        HasUnresolvedActions = job.HasUnresolvedActions(),
                    };

                    singleRoute.Items.Add(item);
                }
            }
            return singleRoute;
        }
    }
}