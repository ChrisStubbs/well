namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.ValueObjects;
    using System;
    using Contracts;
    using Domain.Enums;
    using Domain.Extensions;
    using Models;
    using Services.Contracts;
    using Branch = Domain.Branch;


    public class SingleRouteMapper : ISingleRouteMapper
    {

        public SingleRouteView Map(List<Branch> branches, RouteHeader route, List<Stop> stops, List<Job> jobs, List<Assignee> assignee)
        {
            var singleRoute = new SingleRouteView
            {
                Id = route.Id,
                RouteNumber = route.RouteNumber,
                Branch = branches.Single(x => x.Id == route.RouteOwnerId).BranchName,
                Driver = route.DriverName,
                RouteDate = route.RouteDate
            };

            return AddItems(singleRoute, stops, jobs, assignee);
        }

        private SingleRouteView AddItems(SingleRouteView singleRoute, List<Stop> stops, List<Job> jobs, List<Assignee> assignee)
        {
            
            foreach (var stop in stops)
            {
                var stopJobs = jobs.Where(x => x.StopId == stop.Id).ToArray();

                var stopJobDetails = stopJobs.SelectMany(x => x.JobDetails).ToArray();
               
                foreach (var job in stopJobs)
                {
                    var item = new SingleRouteViewItem();
                    item.JobId = job.Id;
                    item.Stop = stop.DropId;
                    item.StopStatus = stop.StopStatusDescription +" This is Stop Status??";
                    item.StopExceptions = stopJobDetails.Count(x=> !x.IsClean());
                    item.StopClean = stopJobDetails.Count(x => x.IsClean());
                    item.Tba = stopJobs.Sum(j => j.ToBeAdvisedCount);
                    item.StopAssignee = Assignee.GetDisplayNames(assignee.Where(x => x.StopId == stop.Id).ToList());
                    item.Resolution = "TODO:";
                    item.Invoice = job.InvoiceNumber;

                    JobStatus jobType;
                    if (Enum.TryParse(job.JobTypeCode, out jobType))
                    {
                        item.JobType = EnumExtensions.GetDescription(jobType);
                    }
                    item.JobStatus = job.JobStatus;
                    item.JobStatusDescription = job.JobStatus.ToString();
                    item.Cod = job.Cod;
                    item.Pod = job.ProofOfDelivery.HasValue;
                    item.Exceptions = job.JobDetails.Count(x => !x.IsClean());
                    item.Clean = job.JobDetails.Count(x => !x.IsClean());
                    //var creditActions = job.JobDetails.SelectMany(x => x.Actions.Where(y=> y.Action == EventAction.Credit || y.Action == EventAction.CreditAndReorder));

                    //item.Credit = 0; creditActions.Sum(x=> x.Quantity) * 
                    singleRoute.Items.Add(item);
                }
            }
            return singleRoute;
        }
    }
}