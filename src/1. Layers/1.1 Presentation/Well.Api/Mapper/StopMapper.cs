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
        private const int LengthOfBarcode = 18;

        public StopModel Map(List<Branch> branches, RouteHeader route, Stop stop, List<Job> jobs, List<Assignee> assignees)
        {
            var stopModel = new StopModel
            {
                RouteId = route.Id,
                RouteNumber = route.RouteNumber,
                Branch = branches.Single(x => x.Id == route.RouteOwnerId).BranchName,
                BranchId = route.RouteOwnerId,
                Driver = route.DriverName,
                RouteDate = route.RouteDate,
                AssignedTo = Assignee.GetDisplayNames(assignees),
                Tba = jobs.Sum(j => j.ToBeAdvisedCount),
                StopNo = stop.PlannedStopNumber,
                TotalNoOfStopsOnRoute = route.PlannedStops
            };


            return MapItems(stopModel, jobs);
        }

        private StopModel MapItems(StopModel stopModel, List<Job> jobs)
        {
            foreach (var job in jobs)
            {
                var jobType = EnumExtensions.GetValueFromDescription<JobType>(job.JobTypeCode);

                List<JobDetail> jobDetails = job.JobDetails;

                if (jobType == JobType.Tobacco)
                {
                    jobDetails = job.JobDetails
                        .Where(x => x.PhProductCode.Length != LengthOfBarcode)
                        .ToList();
                }
                else if (jobType == JobType.Documents)
                {
                    continue;
                }

                foreach (var line in jobDetails)
                {
                    var item = new StopModelItem()
                    {
                        JobId = job.Id,
                        Invoice = job.InvoiceNumber,
                        Type = jobType.ToString(),
                        Account = job.PhAccount,
                        AccountID = job.PhAccountId,
                        JobDetailId = line.Id,
                        Product = line.PhProductCode,
                        Description = line.ProdDesc,
                        Value = line.SkuGoodsValue,
                        Invoiced = line.OriginalDespatchQty,
                        Delivered = line.DeliveredQty,
                        Damages = line.JobDetailDamages.Sum(x => x.Qty),
                        Shorts = line.ShortQty,
                        Checked = line.IsChecked,
                        HighValue = line.IsHighValue,
                        BarCode = line.SSCCBarcode,
                        LineItemId = line.LineItemId
                    };
                    stopModel.Items.Add(item);
                }
            }

            return stopModel;
        }
    }
}