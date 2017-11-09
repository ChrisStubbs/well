using PH.Shared.Well.Data.EF;
using PH.Well.Domain.Enums;

namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain.ValueObjects;
    using PH.Well.Domain.Extensions;

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
                .Where(x => x.Id == activityId && x.DateDeleted == null)
                .Select(x => new
                {
                    ActivityId = x.Id,
                    ActivityTypeId = x.ActivityTypeId,
                    BranchId = x.Location.Branch.Id,
                    Branch = x.Location.Branch.Name,
                    Job = x.Job
                        .Where(p => p.DateDeleted == null)
                        .Select(y=> new
                        {
                            y.COD,
                            y.ProofOfDelivery,
                            y.ResolutionStatusId,
                            y.OuterDiscrepancyFound,
                            y.TotalOutersShort,
                            y.DetailOutersShort,
                            y.Stop.RouteHeader.DriverName,
                            y.Stop.RouteHeader.RouteDate,
                            Account = y.Stop.Account.FirstOrDefault(),
                            y.JobStatusId

                        })
                        .FirstOrDefault(),
                    ItemNumber = x.DocumentNumber,
                    InitialDocument = x.InitialDocument,
                    x.LocationId
                }).FirstOrDefault();

            var jobTypes = new HybridDictionary(10, false);
            wellEntities.JobType
                .Where(p => p.Id != (int)PH.Well.Domain.Enums.JobType.NotDefined)
                .Select(p => new { p.Code, p.Description, p.Abbreviation})
                .ToList()
                .ForEach(p => jobTypes.Add(p.Code, $"{p.Description}({p.Abbreviation})"));

            // Grid details
            var details = GetById(activityId)
                .Select(p =>
                {
                    p.HasUnresolvedActions = p.HasUnresolvedActions();
                    p.Type = (string)jobTypes[p.JobType];
                    p.CompletedOnPaper = p.JobStatusId == (int)Domain.Enums.JobStatus.CompletedOnPaper;
                    return p;
                })
                .ToList();

            var users = wellEntities.Activity
                .Where(x => x.Id == activitySource.ActivityId && x.DateDeleted == null)
                .Select(x => new
                {
                    Users = x.Job
                        .Where(p => p.DateDeleted == null)
                        .SelectMany(y => y.UserJob.Select(u => u.User.Name))
                })
                .SelectMany(p => p.Users)
                .Distinct();

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
                ResolutionStatus = activitySource.Job.ResolutionStatusId.GetValueOrDefault(),
                LocationId = (int)activitySource.LocationId,
                Assignees = users.ToList(),
                Details = details,
                InitialDocument = activitySource.InitialDocument
            };

            return result;
        }
        private IList<ActivitySourceDetail> GetById(int id)
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.ActivityDetails)
                   .AddParameter("Id", id, DbType.Int32)
                   .Query<ActivitySourceDetail>().ToList();
        }

        // Helper methods below should be refactored use constants and be centralized.
        private bool HasUnresolvedActions(string lineDeliveryStatus, int? shortTotal, int? damageTotal, int? originalDespatchQuantity, Domain.Enums.ResolutionStatus resolutionStatus)
        {
            if ((resolutionStatus & Domain.Enums.ResolutionStatus.Closed) == Domain.Enums.ResolutionStatus.Closed)
            {
                return false;
            }
            
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

