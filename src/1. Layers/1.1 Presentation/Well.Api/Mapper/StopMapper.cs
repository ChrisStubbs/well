namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.Extensions;
    using Domain.ValueObjects;
    using Models;
    using Branch = Domain.Branch;

    public class StopMapper : IStopMapper
    {
        

        public StopModel Map(List<Branch> branches, RouteHeader route, Stop stop, List<Job> jobs, List<Assignee> assignees,
            IEnumerable<JobDetailLineItemTotals> jobDetailTotalsPerStop)
        {
            var jobGroupToBeAdvised = jobs.GroupBy(j => new { j.OuterCount, j.ToBeAdvisedCount })
                                        .Select(y => new ToBeAdvisedGroup()
                                                    {
                                                        OuterCountId = y.Key.OuterCount.GetValueOrDefault(),
                                                        ToBeAdvisedCount = y.Key.ToBeAdvisedCount
                                                    }).ToList();

            var stopModel = new StopModel
            {
                RouteId = route.Id,
                RouteNumber = route.RouteNumber,
                Branch = branches.Single(x => x.Id == route.RouteOwnerId).BranchName,
                BranchId = route.RouteOwnerId,
                Driver = route.DriverName,
                RouteDate = route.RouteDate,
                AssignedTo = Assignee.GetDisplayNames(assignees),
                Tba = jobGroupToBeAdvised.Sum(j => j.ToBeAdvisedCount),
                StopNo = stop.PlannedStopNumber,
                TotalNoOfStopsOnRoute = route.PlannedStops,
                Items = MapItems(jobs, jobDetailTotalsPerStop)
            };
            
            return stopModel;
        }

        private IList<StopModelItem> MapItems(List<Job> jobs, IEnumerable<JobDetailLineItemTotals> jobDetailTotalsPerStop)
        {
            return jobs
                .Select(p => new
                {
                    jobType = EnumExtensions.GetValueFromDescription<JobType>(p.JobTypeCode),
                    job = p
                })
                .Where(p => p.jobType != JobType.Documents)
                .SelectMany(p =>
                {
                    var jobDetails = p.job.JobDetails;

                    if (p.jobType == JobType.Tobacco)
                    {
                        jobDetails = jobDetails
                        .Where(x => !x.IsTobaccoBag())
                        .ToList();
                    }

                    return jobDetails
                        .Select(line => new
                        {
                            DetailId = line.Id,
                            StopModelItem = new StopModelItem
                            {
                                JobId = p.job.Id,
                                Invoice = p.job.InvoiceNumber,
                                Type = p.job.JobType,
                                JobTypeAbbreviation = p.job.JobTypeAbbreviation,
                                Account = p.job.PhAccount,
                                AccountID = p.job.PhAccountId,
                                JobDetailId = line.Id,
                                Product = line.PhProductCode,
                                Description = line.ProdDesc,
                                Value =  line.NetPrice?? 0,
                                Invoiced = line.OriginalDespatchQty,
                                Delivered = line.DeliveredQty,
                                Checked = line.IsChecked,
                                HighValue = line.IsHighValue,
                                BarCode = line.SSCCBarcode,
                                LineItemId = line.LineItemId,
                                Resolution = p.job.ResolutionStatus.Description,
                                ResolutionId = p.job.ResolutionStatus.Value,
                                GrnProcessType = p.job.GrnProcessType ?? 0,
                                HasUnresolvedActions = HasUnresolvedAction(p.job, line.LineItemId),
                                GrnNumber = p.job.GrnNumber
                            }
                        })
                        .ToList();
                })
                .Select(line => 
                {
                    var totals = jobDetailTotalsPerStop.FirstOrDefault(p => p.JobDetailId == line.DetailId) ?? new JobDetailLineItemTotals();

                    line.StopModelItem.Damages = totals.DamageTotal;
                    line.StopModelItem.Shorts = totals.ShortTotal;
                    line.StopModelItem.Bypassed = totals.BypassTotal;

                    return line.StopModelItem;
                })
                .ToList();
        }

        private static bool HasUnresolvedAction(Job job, int lineItemId)
        {
            var lineItems = job.LineItems.Where(x => x.Id == lineItemId).ToArray();
            if (lineItems.Any())
            {
                return lineItems.Any(x => x.LineItemActions.Any(y => y.DeliveryAction == DeliveryAction.NotDefined));
            }
            return false;
        }
    }
}