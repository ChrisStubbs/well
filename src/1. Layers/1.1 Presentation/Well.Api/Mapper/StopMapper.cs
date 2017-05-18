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
                    jobDetails = job.JobDetails.Where(x => x.PhProductCode.Length != LengthOfBarcode).ToList();
                }

                foreach (var line in jobDetails)
                {
                    var item = new StopModelItem();
                    item.JobId = job.Id;
                    item.Invoice = job.InvoiceNumber;
                    item.Type = jobType.ToString().Substring(0, 1);
                    item.Account = job.PhAccount;
                    item.JobDetailId = line.Id;
                    item.Product = line.PhProductCode;
                    item.Description = line.ProdDesc;
                    item.Value = line.SkuGoodsValue;
                    item.Invoiced = line.OriginalDespatchQty;
                    item.Delivered = line.DeliveredQty;
                    item.Damages = line.JobDetailDamages.Sum(x => x.Qty);
                    item.Shorts = line.ShortQty;
                    item.Checked = line.IsChecked;
                    item.HighValue = line.IsHighValue;
                    item.BarCode = line.SSCCBarcode;
                    item.LineItemId = line.LineItemId;
                    stopModel.Items.Add(item);
                }

            }

            return stopModel;
        }
    }
}