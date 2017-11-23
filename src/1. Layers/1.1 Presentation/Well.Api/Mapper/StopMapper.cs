using PH.Well.Common.Contracts;
using PH.Well.Services.Contracts;

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
        private readonly IJobService jobService;
        private readonly IUserNameProvider userNameProvider;
        private readonly ILookupService lookupService;

        public StopMapper(IJobService jobService, IUserNameProvider userNameProvider, ILookupService lookupService)
        {
            this.jobService = jobService;
            this.userNameProvider = userNameProvider;
            this.lookupService = lookupService;
        }

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
                Items = MapItems(jobs, jobDetailTotalsPerStop.ToList())
            };

            return stopModel;
        }

        private IList<StopModelItem> MapItems(List<Job> jobs, IList<JobDetailLineItemTotals> jobDetailTotalsPerStop)
        {
            var jobTypes = this.lookupService.GetLookup(LookupType.JobType)
                .ToDictionary(k => int.Parse(k.Key), v => v.Value);

            return jobs
                .Where(p => p.JobType != JobType.Documents)
                .SelectMany(p =>
                {
                    var jobDetails = p.JobDetails
                        .Join(p.LineItems,
                            l => new { l.LineNumber, Product = l.PhProductCode },
                            r => new { r.LineNumber, Product = r.ProductCode },
                            (det, line) => new
                            {
                                det.Id,
                                det.PhProductCode,
                                det.ProdDesc,
                                Value = det.NetPrice ?? 0,
                                det.OriginalDespatchQty,
                                det.DeliveredQty,
                                det.IsChecked,
                                det.IsHighValue,
                                det.SSCCBarcode,
                                det.LineItemId,
                                IsTobaccoBag = det.IsTobaccoBag(),
                                HasLineItemActions = line.LineItemActions.Where(lia => lia.DateDeleted == null).Count() > 0,
                                UpliftAction = det.UpliftAction.GetValueOrDefault(),
                                JobTypeId = (int)p.JobType
                            });

                    if (p.JobType == JobType.Tobacco)
                    {
                        jobDetails = jobDetails
                        .Where(x => !x.IsTobaccoBag);
                    }

                    return jobDetails
                        .Select(line => new
                        {
                            DetailId = line.Id,
                            StopModelItem = new StopModelItem
                            {
                                JobId = p.Id,
                                Invoice = p.InvoiceNumber,
                                InvoiceId = p.ActivityId,
                                Type = jobTypes[(int)p.JobType],
                                JobTypeAbbreviation = p.JobTypeAbbreviation,
                                Account = p.PhAccount,
                                AccountID = p.PhAccountId,
                                JobDetailId = line.Id,
                                Product = line.PhProductCode,
                                Description = line.ProdDesc,
                                Value = line.Value,
                                Invoiced = line.OriginalDespatchQty,
                                Delivered = line.DeliveredQty,
                                Checked = line.IsChecked,
                                HighValue = line.IsHighValue,
                                BarCode = line.SSCCBarcode,
                                LineItemId = line.LineItemId,
                                Resolution = p.ResolutionStatus.Description,
                                ResolutionId = p.ResolutionStatus.Value,
                                GrnProcessType = p.GrnProcessType ?? 0,
                                HasUnresolvedActions = p.HasUnresolvedActions(line.LineItemId),
                                GrnNumber = p.GrnNumber,
                                CanEditReason = jobService.CanEdit(p, userNameProvider.GetUserName()),
                                LocationId = p.LocationId,
                                CompletedOnPaper = p.JobStatus == JobStatus.CompletedOnPaper,
                                HasLineItemActions = line.HasLineItemActions,
                                UpliftAction = line.UpliftAction,
                                JobTypeId = line.JobTypeId
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
    }
}