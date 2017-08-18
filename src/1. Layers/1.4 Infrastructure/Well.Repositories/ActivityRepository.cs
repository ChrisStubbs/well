using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PH.Shared.Well.Data.EF;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using ExceptionType = PH.Well.Domain.Enums.ExceptionType;
using Job = PH.Shared.Well.Data.EF.Job;
using JobDetail = PH.Shared.Well.Data.EF.JobDetail;
using JobType = PH.Well.Domain.Enums.JobType;

namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain.ValueObjects;
    using System.Data.Entity;

    public class ActivityRepository : IActivityRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;
        private readonly WellEntities wellEntities;

        public ActivityRepository(ILogger logger, IDapperReadProxy dapperReadProxy,WellEntities wellEntities)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
            this.wellEntities = wellEntities;
        }

        #region GET ACTIVITY SOURCE USING EF

        private ActivitySource GetActivitySourceByIdEF(int activityId)
        {
            // Header information
            var activitySource = wellEntities.Activity
                .Include("Job")
                .Include("Job.Stop")
                .Include("Job.Stop.RouteHeader")
                .Include("Job.Stop.Account")
                .Where(x => x.Id == activityId)
                .Select(x => new
                {
                    ActivityId = x.Id,
                    ActivityTypeId = x.ActivityTypeId,
                    BranchId = x.Location.Branch.Id,
                    Branch = x.Location.Branch.Name,
                    Job = x.Job.Select(y=> new
                    {
                        y.COD,
                        y.ProofOfDelivery,
                        y.ResolutionStatusId,
                        y.OuterDiscrepancyFound,
                        y.TotalOutersShort,
                        y.DetailOutersShort,
                        y.Stop.RouteHeader.DriverName,
                        y.Stop.RouteHeader.RouteDate,
                        Account = y.Stop.Account.FirstOrDefault()

                    }).FirstOrDefault(),
                    ItemNumber = x.DocumentNumber,
                    x.LocationId
                }).FirstOrDefault();

            // Grid details
            var details = wellEntities.Activity
                .Where(x => x.Id == activitySource.ActivityId)
                .Select(x => new
                {
                    ActivityId = x.Id,
                    Users = x.Job.SelectMany(y => y.UserJob.Select(u => u.User.Name)).Distinct(),
                    LineDetail = x.Job.SelectMany(y => y.JobDetail.Select(z => new
                    {
                        Product = z.LineItem.ProductCode,
                        Type = y.JobTypeCode,
                        Barcode = z.SSCCBarcode,
                        Description = z.LineItem.ProductDescription,
                        Value = z.NetPrice,
                        Expected = z.OriginalDespatchQty,
                        LineDeliveryStatus = z.LineDeliveryStatus,
                        z.IsHighValue,
                        y.Stop,
                        JobId = y.Id,
                        JobType = y.JobTypeCode,
                        LineItemId = (int?) z.LineItem.Id,
                        ResolutionStatus = y.ResolutionStatusId,
                        OriginalDespatchQuantity = z.OriginalDespatchQty,

                        // This method can be reused globally with EF. check http://www.albahari.com/nutshell/linqkit.aspx
                        Totals = new
                        {
                            z.Id,
                            BypassTotal = z.LineItem.LineItemAction
                                .Where(l => l.ExceptionType.Id == (int) ExceptionType.Bypass)
                                .Sum(l => (int?) l.Quantity),
                            DamageTotal = z.LineItem.LineItemAction
                                .Where(l => l.ExceptionType.Id == (int) ExceptionType.Damage)
                                .Sum(l => (int?) l.Quantity),
                            ShortTotal = z.LineItem.LineItemAction
                                .Where(l => l.ExceptionType.Id == (int) ExceptionType.Short)
                                .Sum(l => (int?) l.Quantity),
                            TotalExceptions = z.LineItem.LineItemAction.Count(),
                        }
                    }))
                }).FirstOrDefault();


            var result = new ActivitySource
            {
                ActivityId = activitySource.ActivityId,
                Branch = activitySource.Branch,
                BranchId = activitySource.BranchId,
                AccountAddress = $"{activitySource.Job.Account.Address1} {activitySource.Job.Account.Address2} {activitySource.Job.Account.PostCode}",
                AccountName = activitySource.Job.Account.Name,
                PrimaryAccount = activitySource.Job.Account.Code,
                ItemNumber = activitySource.ItemNumber,
                Cod = IsCod(activitySource.Job.COD),
                IsInvoice = IsInvoice(activitySource.ActivityTypeId),
                Pod = IsPod(activitySource.Job.ProofOfDelivery),
                Driver =  activitySource.Job.DriverName,
                Date =  activitySource.Job.RouteDate.Value,
                Tba = GetTba(activitySource.Job.OuterDiscrepancyFound,activitySource.Job.TotalOutersShort,activitySource.Job.DetailOutersShort),
                ResolutionStatus = activitySource.Job.ResolutionStatusId,
                LocationId = (int)activitySource.LocationId,
                Assignees = details.Users.ToList(),
                Details = details.LineDetail.Select(x => new ActivitySourceDetail
                {
                    ActivityId = details.ActivityId,
                    Product = x.Product,
                    Type = x.Type,
                    BarCode = x.Barcode,
                    Description = x.Description,
                    Value = (decimal) x.Value.GetValueOrDefault(),
                    Expected = x.Expected.GetValueOrDefault(),
                    Damaged = x.Totals.DamageTotal.GetValueOrDefault(),
                    Shorts = x.Totals.ShortTotal.GetValueOrDefault(),
                    HighValue = x.IsHighValue,
                    StopId = x.Stop.Id,
                    Stop = x.Stop.DropId,
                    StopDate = x.Stop.DeliveryDate.GetValueOrDefault(),
                    JobId = x.JobId,
                    ResolutionStatus = x.ResolutionStatus,
                    LineItemId = x.LineItemId ?? -1,
                    HasUnresolvedActions = HasUnresolvedActions(x.LineDeliveryStatus,x.Totals.ShortTotal,x.Totals.DamageTotal,x.OriginalDespatchQuantity),
                    
                }).ToList()
            };

            return result;
        }

        // Helper methods below should be refactored use constants and be centralized.
        private bool HasUnresolvedActions(string lineDeliveryStatus, int? shortTotal, int? damageTotal, int? originalDespatchQuantity)
        {
            switch (lineDeliveryStatus)
            {
                case "Delivered": return true;
                case "Exception": return true;
                case "Unknown":
                    return (shortTotal > 0 || damageTotal == originalDespatchQuantity);
                default: return false;
            }
        }
       

        private int GetTba(bool? outerDiscrepancyFound, int? totalOutersShort, int? detailOutersShort)
        {
            return (outerDiscrepancyFound.HasValue && outerDiscrepancyFound.Value)
                ? totalOutersShort.GetValueOrDefault() - detailOutersShort.GetValueOrDefault()
                : 0;
        }

        private bool IsPod(int? proofOfDelivery)
        {
            return proofOfDelivery.HasValue && (proofOfDelivery == (int) ProofOfDelivery.CocaCola ||
                                                proofOfDelivery == (int) ProofOfDelivery.Lucozade);
        }

        private bool IsCod(string cod)
        {
            return (cod == "Cash" || cod == "Cheque" || cod == "Card");
        }

        private bool IsInvoice(int activityTypeId)
        {
            return activityTypeId == 1;
        }

        #endregion GET ACTIVITY SOURCE USING EF

        public ActivitySource GetActivitySourceById(int activityId)
        {
            return GetActivitySourceByIdEF(activityId);
        }

        public ActivitySource GetActivitySourceByDocumentNumber(string documentNumber, int branchId)
        {
            var activitySource = new ActivitySource();

            dapperReadProxy.WithStoredProcedure(StoredProcedures.ActivityGetByDocumentNumber)
                .AddParameter("documentNumber", documentNumber, DbType.String)
                .AddParameter("branchId", branchId, DbType.Int32)
                .QueryMultiple(x => activitySource = GetActivityFromGrid(x));

            return activitySource;
        }

        public ActivitySource GetActivityFromGrid(SqlMapper.GridReader grid)
        {
            var activitySource = grid.Read<ActivitySource>().FirstOrDefault();
            var details = grid.Read<ActivitySourceDetail>().ToList();
            var assignees = grid.Read<string>().ToList();
            if (activitySource != null)
            {
                activitySource.Details = details.Where(x => x.ActivityId == activitySource.ActivityId).ToList();
                activitySource.Assignees = assignees;
            }

            return activitySource;
        }

    }
}

