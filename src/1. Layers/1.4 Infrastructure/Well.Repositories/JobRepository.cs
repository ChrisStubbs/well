using System;
using PH.Well.Domain.Extensions;
using PH.Shared.Well.Data.EF;

namespace PH.Well.Repositories
{
    using Dapper;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    public class JobRepository : DapperRepository<Job, int>, IJobRepository
    {
        private readonly WellEntities wellEntities;

        public JobRepository(ILogger logger, IWellDapperProxy dapperProxy, IUserNameProvider userNameProvider, WellEntities wellEntities)
            : base(logger, dapperProxy, userNameProvider)
        {
            this.wellEntities = wellEntities;
        }

        public Job GetById(int id)
        {
            return GetByIds(new List<int>() { id }).FirstOrDefault();
        }

        public IEnumerable<Job> GetByStopId(int id)
        {
            var jobIds = dapperProxy.WithStoredProcedure(StoredProcedures.JobGetByStopId)
                   .AddParameter("StopId", id, DbType.Int32)
                   .Query<int>();

            return GetByIds(jobIds);
        }
        
        private IQueryable<JobDetailLineItemTotals> ToLineItemTotals(IQueryable<PH.Shared.Well.Data.EF.JobDetail> jobDetails)
        {
            return jobDetails
                .Where(p => p.Job.DateDeleted == null
                    && p.Job.ResolutionStatusId.Value > ResolutionStatus.Imported.Value
                    && p.Job.Stop.DateDeleted == null
                    && p.Job.Stop.RouteHeader.DateDeleted == null)
                .Select(x => new JobDetailLineItemTotals
                {
                    JobDetailId = x.Id,
                    JobId = x.JobId,
                    StopId = x.Job.StopId,
                    RouteId = x.Job.Stop.RouteHeaderId,
                    BypassTotal = x.LineItem.LineItemAction
                        .Where(y => y.ExceptionType.Id == (int)ExceptionType.Bypass && y.DateDeleted == null)
                        .Sum(y => (int?)y.Quantity) ?? 0,
                    DamageTotal = x.LineItem.LineItemAction
                        .Where(y => y.ExceptionType.Id == (int)ExceptionType.Damage && y.DateDeleted == null)
                        .Sum(y => (int?)y.Quantity) ?? 0,
                    ShortTotal = x.LineItem.LineItemAction
                        .Where(y => y.ExceptionType.Id == (int)ExceptionType.Short && y.DateDeleted == null)
                        .Sum(y => (int?)y.Quantity) ?? 0,
                    TotalExceptions = x.LineItem.LineItemAction.Where(y => y.DateDeleted == null).Count(),
                });
        }

        public IEnumerable<JobDetailLineItemTotals> JobDetailTotalsPerStop(int stopId)
        {
            var totals = ToLineItemTotals(wellEntities.JobDetail.Where(x => x.Job.StopId == stopId));
            return totals;
        }

        public IList<JobDetailLineItemTotals> JobDetailTotalsPerRouteHeader(int routeHeaderId)
        {
            return ToLineItemTotals(wellEntities.JobDetail.Where(x => x.Job.Stop.RouteHeaderId == routeHeaderId))
                .ToList();
        }

        public IEnumerable<JobDetailLineItemTotals> JobDetailTotalsPerJobs(IEnumerable<int> jobIds)
        {
            return ToLineItemTotals(wellEntities.JobDetail.Where(x => jobIds.Contains(x.Job.Id)));
        }

        public IEnumerable<JobStop> GetJobStopsByRouteHeaderId(int routeHeaderId)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.JobGetByRouteHeaderId)
                .AddParameter("RouteHeaderId", routeHeaderId, DbType.Int32)
                .Query<JobStop>();
        }

        public IEnumerable<Job> GetByRouteHeaderId(int routeHeaderId)
        {
            var jobIds = GetJobStopsByRouteHeaderId(routeHeaderId).Select(x => x.JobId);

            return GetByIds(jobIds);
        }

        public Job GetJobByRefDetails(string jobTypeCode, string phAccount, string pickListRef, int stopId)
        {
            var jobIds = dapperProxy.WithStoredProcedure(StoredProcedures.JobGetByRefDetails)
                .AddParameter("JobTypeCode", jobTypeCode, DbType.String)
                .AddParameter("PHAccount", phAccount, DbType.String)
                .AddParameter("PickListRef", pickListRef, DbType.String)
                .AddParameter("StopId", stopId, DbType.Int32)
                .Query<int>();

            return GetByIds(jobIds).SingleOrDefault();
        }

        protected override void SaveNew(Job entity)
        {
            entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobInsert)
                .AddParameter("Sequence", entity.Sequence, DbType.Int32)
                .AddParameter("JobTypeId", (byte)entity.JobType, DbType.Byte)
                .AddParameter("JobTypeCode", entity.JobTypeCode, DbType.String)
                .AddParameter("PHAccount", entity.PhAccount, DbType.String)
                .AddParameter("PickListRef", entity.PickListRef, DbType.String)
                .AddParameter("InvoiceNumber", entity.InvoiceNumber, DbType.String)
                .AddParameter("CustomerRef", entity.CustomerRef, DbType.String)
                .AddParameter("OrderDate", entity.OrderDate, DbType.DateTime)
                .AddParameter("RoyaltyCode", entity.RoyaltyCode, DbType.String)
                .AddParameter("RoyaltyCodeDesc", entity.RoyaltyCodeDesc, DbType.String)
                .AddParameter("OrdOuters", entity.OrdOuters ?? 0, DbType.Int16)
                .AddParameter("InvOuters", entity.InvOuters ?? 0, DbType.Int16)
                .AddParameter("ColOuters", entity.ColOuters ?? 0, DbType.Int16)
                .AddParameter("ColBoxes", entity.ColBoxes ?? 0, DbType.Int16)
                .AddParameter("ReCallPrd", entity.ReCallPrd, DbType.Boolean)
                .AddParameter("AllowSOCrd", entity.AllowSoCrd, DbType.Boolean)
                .AddParameter("COD", entity.Cod, DbType.String)
                .AddParameter("GrnProcessingType", entity.GrnProcessType ?? 0, DbType.Int16)
                .AddParameter("AllowReOrd", entity.AllowReOrd, DbType.Boolean)
                .AddParameter("SandwchOrd", entity.SandwchOrd, DbType.Boolean)
                .AddParameter("PerformanceStatusId", (int)entity.PerformanceStatus, DbType.Int16)
                .AddParameter("Reason", entity.JobByPassReason, DbType.String)
                .AddParameter("StopId", entity.StopId, DbType.Int32)
                .AddParameter("ActionLogNumber", entity.ActionLogNumber, DbType.String)
                .AddParameter("OuterCount", entity.OuterCount, DbType.Int16)
                .AddParameter("OuterDiscrepancyFound", entity.OuterDiscrepancyFound, DbType.String)
                .AddParameter("TotalOutersOver", entity.TotalOutersOver, DbType.Int16)
                .AddParameter("TotalOutersShort", entity.TotalOutersShort, DbType.Int16)
                .AddParameter("Picked", entity.Picked, DbType.Boolean)
                .AddParameter("InvoiceValue", entity.InvoiceValue, DbType.Decimal)
                .AddParameter("ProofOfDelivery", entity.ProofOfDelivery, DbType.Int16)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("CreatedDate", entity.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime)
                .AddParameter("JobStatusId", (int)entity.JobStatus, DbType.Int16)
                .AddParameter("ResolutionStatusId", entity.ResolutionStatus.Value, DbType.Int16)
                .AddParameter("WellStatusId", entity.JobStatus.ToWellStatus(), DbType.Int16)
                .Query<int>().FirstOrDefault();
        }

        public void DeleteJobById(int id)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobDeleteById)
                .AddParameter("JobId", id, DbType.Int32)
                .Execute();
        }

        protected override void UpdateExisting(Job entity)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.JobUpdate)
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("StopId", entity.StopId, DbType.Int32)
                .AddParameter("Reason", entity.JobByPassReason, DbType.String)
                .AddParameter("PerformanceStatus", (int)entity.PerformanceStatus, DbType.Int32)
                .AddParameter("InvoiceNumber", entity.InvoiceNumber, DbType.String)
                .AddParameter("Sequence", entity.Sequence, DbType.Int32)
                .AddParameter("JobTypeCode", entity.JobTypeCode, DbType.String)
                .AddParameter("PhAccount", entity.PhAccount, DbType.String)
                .AddParameter("GrnNumber", entity.GrnNumber, DbType.String)
                .AddParameter("CustomerRef", entity.CustomerRef, DbType.String)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime)
                .AddParameter("Picked", entity.Picked, DbType.Boolean)
                .AddParameter("OrdOuters", entity.OrdOuters, DbType.Int32)
                .AddParameter("InvOuters", entity.InvOuters, DbType.Int32)
                .AddParameter("AllowSoCrd", entity.AllowSoCrd, DbType.Boolean)
                .AddParameter("Cod", entity.Cod, DbType.String)
                .AddParameter("AllowReOrd", entity.AllowReOrd, DbType.Boolean)
                .AddParameter("JobStatusId", (int)entity.JobStatus, DbType.Int16)
                .AddParameter("OuterCount", entity.OuterCount, DbType.Int32)
                .AddParameter("OuterDiscrepancyFound", entity.OuterDiscrepancyFound, DbType.Boolean)
                .AddParameter("TotalOutersOver", entity.TotalOutersOverUpdate, DbType.Int32)
                .AddParameter("TotalOutersShort", entity.TotalOutersShort, DbType.Int32)
                .AddParameter("InvoiceValue", entity.InvoiceValueUpdate, DbType.Decimal)
                .AddParameter("DetailOutersOver", entity.DetailOutersOverUpdate, DbType.Int16)
                .AddParameter("DetailOutersShort", entity.DetailOutersShort, DbType.Int16)
                .AddParameter("ResolutionStatusId", entity.ResolutionStatus.Value, DbType.Int16)
                .AddParameter("WellStatusId", (int)entity.WellStatus, DbType.Int16)
                .Execute();
        }

        public IEnumerable<PodActionReasons> GetPodActionReasonsById(int pdaCreditReasonId)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.JobGetCreditActionReasons)
                   .AddParameter("PDACreditReasonId", pdaCreditReasonId, DbType.Int32)
                   .Query<PodActionReasons>();
        }

        public void SaveGrn(int jobId, string grn)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.SaveGrn)
                .AddParameter("JobId", jobId, DbType.Int32)
                .AddParameter("Grn", grn, DbType.String)
                .Execute();
        }

        public void SetJobResolutionStatus(int jobId, string status)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.JobResolutionStatusInsert)
                .AddParameter("Status", status, DbType.String)
                .AddParameter("JobId", jobId, DbType.Int32)
                .AddParameter("CreatedBy", this.CurrentUser, DbType.String)
                .Execute();
        }

        public void SetJobToSubmittedStatus(int jobId)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.JobSetToStatus)
                .AddParameter("jobId", jobId, DbType.Int32)
                .AddParameter("status", PerformanceStatus.Submitted, DbType.Int16)
                .Execute();
        }

        public IEnumerable<Job> GetJobsByBranchAndInvoiceNumber(int jobId, int branchId, string invoiceNumber)
        {
            var ids = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobGetByBranchAndInvoiceNumberWithFullObjectGraph)
                    .AddParameter("jobId", jobId, DbType.Int32)
                    .AddParameter("branchId", branchId, DbType.Int32)
                    .AddParameter("invoiceNumber", invoiceNumber, DbType.String)
                    .Query<int>();

            return GetByIds(ids);
        }

        public IEnumerable<Job> GetByIds(IEnumerable<int> jobIds)
        {
            IEnumerable<Job> jobs = new List<Job>();

            this.dapperProxy.WithStoredProcedure(StoredProcedures.JobGetByIds)
                .AddParameter("Ids", jobIds.Distinct().ToList().ToIntDataTables("Ids"), DbType.Object)
                .QueryMultiple(x => jobs = GetJobsByGrid(x));

            return jobs;
        }

        private IEnumerable<Job> GetJobsByGrid(SqlMapper.GridReader gridReader)
        {
            var jobs = gridReader.Read<Job>().ToList();
            var jobDetails = gridReader.Read<JobDetail>().ToList();
            var jobDetailsDamages = gridReader.Read<JobDetailDamage>().ToList();
            var jobResolutionHistory = gridReader.Read<JobResolutionStatus>().ToList();
            foreach (var job in jobs)
            {
                job.JobDetails = jobDetails.Where(x => x.JobId == job.Id).ToList();
                job.ResolutionStatusHistory = jobResolutionHistory.Where(x => x.JobId == job.Id);

                foreach (JobDetail jobDetail in job.JobDetails)
                {
                    jobDetail.JobDetailDamages = jobDetailsDamages.Where(x => x.JobDetailId == jobDetail.Id).ToList();
                }
            }
            return jobs;
        }

        public void UpdateStatus(int jobId, JobStatus status)
        {

            this.dapperProxy.WithStoredProcedure(StoredProcedures.JobUpdateStatus)
                .AddParameter("Id", jobId, DbType.Int32)
                .AddParameter("StatusId", (int)status, DbType.Int16)
                .Execute();
        }

        public IEnumerable<JobRoute> GetJobsRoute(IEnumerable<int> jobIds)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.GetJobRoutesByJobIds)
                   .AddParameter("JobIds", jobIds.ToList().ToIntDataTables("Ids"), DbType.Object)
                   .Query<JobRoute>();
        }

        public JobRoute GetJobRoute(int jobId)
        {
            return GetJobsRoute(new[] { jobId }).FirstOrDefault();
        }

        public void SaveJobResolutionStatus(Job job)
        {
            this.SetJobResolutionStatus(job.Id, job.ResolutionStatus.Description);
        }

        public IEnumerable<JobToBeApproved> GetJobsToBeApproved()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.JobsToBeApproved)
                    .Query<JobToBeApproved>();
        }

        public IEnumerable<Job> GetJobsByResolutionStatus(ResolutionStatus resolutionStatus)
        {
            var jobIds = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobsToBeApproved)
                .AddParameter("ResolutionStatusId", resolutionStatus.Value, DbType.Int32)
                .Query<int>();

            return GetByIds(jobIds);
        }

        public IEnumerable<Job> GetJobsByLineItemIds(IEnumerable<int> lineItemIds)
        {
            var jobIds = this.dapperProxy.WithStoredProcedure(StoredProcedures.GetJobIdsByLineItemIds)
                .AddParameter("Ids", lineItemIds.ToList().ToIntDataTables("Ids"), DbType.Object)
                .Query<int>();

            return GetByIds(jobIds);
        }

        public IEnumerable<int> GetJobsWithLineItemActions(IEnumerable<int> jobIds)
        {
            var idsForAction = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobGetWithLineItemActions)
              .AddParameter("Ids", jobIds.ToList().ToIntDataTables("Ids"), DbType.Object)
              .Query<int>();

            return idsForAction;
        }

        public Dictionary<int, string> GetPrimaryAccountNumberByRouteHeaderId(int routeHeaderId)
        {
            var strSQL = "select j.id, a.Code " +
                         "from Job j " +
                         "INNER JOIN Stop s ON j.StopId = s.id AND s.RouteHeaderId = @RouteHeaderId " +
                         "INNER JOIN Account a ON a.StopId = s.Id ";
            var result = new Dictionary<int, string>();

            Action<IDataReader> callBack = reader =>
            {
                while (reader.Read())
                {
                    result.Add(reader.GetInt32(0), reader.GetString(1));
                }
            };

            this.dapperProxy.ExecuteSql(strSQL, new { RouteHeaderId = routeHeaderId }, callBack);

            return result;
        }

        public IEnumerable<int> GetExistingJobsIdsIncludingSoftDeleted(int branchId, IEnumerable<Job> jobs)
        {
            var data = jobs.Select(p => new
            {
                p.PhAccount,
                p.PickListRef,
                p.JobTypeCode
            }).ToList().ToDataTables();

            return this.dapperProxy.WithStoredProcedure(StoredProcedures.GetJobIdsByBranchAccountPickListRefAndJobType)
                .AddParameter("BranchId", branchId, DbType.Int32)
                .AddParameter("IdentifyJobTable", data, DbType.Object)
                .Query<int>();
        }

        public void CascadeSoftDeleteJobs(IList<int> jobIds, bool deletedByImport = false)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobsCascadeSoftDelete)
                .AddParameter("JobIds", jobIds.ToIntDataTables("JobIds"), DbType.Object)
                .AddParameter("DateDeleted", DateTime.Now, DbType.DateTime)
                .AddParameter("UpdatedBy", CurrentUser, DbType.String)
                .AddParameter("DeletedByImport", deletedByImport, DbType.Boolean)
                .Execute();
        }

        public void ReinstateJobsSoftDeletedByImport(IList<int> jobIds)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobsReinstateSoftDeletedByImport)
                .AddParameter("JobIds", jobIds.ToIntDataTables("JobIds"), DbType.Object)
                .AddParameter("UpdatedBy", CurrentUser, DbType.String)
                .Execute();
        }

        public void JobsSetResolutionStatusClosed(IList<int> jobIds)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.CleanJobsSetResolutionStatusClosed)
                .AddParameter("JobIds", jobIds.ToIntDataTables("JobIds"), DbType.Object)
                .AddParameter("DateDeleted", DateTime.Now, DbType.DateTime)
                .AddParameter("UpdatedBy", CurrentUser, DbType.String)
                .Execute();
        }

        public void UpdateWellStatus(Job job)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.JobUpdateWellStatus)
                .AddParameter("Id", job.Id, DbType.Int32)
                .AddParameter("WellStatusId", (int) job.WellStatus, DbType.Int16)
                .Execute();
        }

        public Job GetForWellStatusCalculationById(int jobId)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.JobGetForWellStatusCalculationById)
                .AddParameter("Id", jobId, DbType.Int32)
                .Query<Job>()
                .SingleOrDefault();
        }
    }
}
