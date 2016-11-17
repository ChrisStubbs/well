﻿namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain;
    using Domain.Enums;

    public class JobDetailRepository : DapperRepository<JobDetail, int>, IJobDetailRepository
    {
        public JobDetailRepository(ILogger logger, IWellDapperProxy dapperProxy) : 
            base(logger, dapperProxy) {}

        public JobDetail GetById(int id)
        {
            return Get(id, null, null).FirstOrDefault();
        }

        public IEnumerable<JobDetail>  GetByJobId(int jobId)
        {
            return Get(null, jobId, null);
        }

        public JobDetail GetByJobLine(int jobId, int lineNumber)
        {
            return Get(null, jobId, lineNumber).FirstOrDefault();
        }

        private IEnumerable<JobDetail> Get(int? id, int? jobId, int? lineNumber)
        {
            var jobDetails = new List<JobDetail>();
            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailGet)
                .AddParameter("Id", id, DbType.Int32)
                .AddParameter("JobId", jobId, DbType.Int32)
                .AddParameter("LineNumber", lineNumber, DbType.Int32)
                .QueryMultiple<JobDetail>(g => jobDetails = GetFromGrid(g));

            return jobDetails;
        }

        private List<JobDetail> GetFromGrid(SqlMapper.GridReader grid)
        {
            var jobDetails = grid.Read<JobDetail>().ToList();
            var jobDetailDamages = grid.Read<JobDetailDamage>().ToList();
            var actions = grid.Read<JobDetailAction>().ToList();

            foreach (var jobDetail in jobDetails)
            {
                jobDetail.JobDetailDamages =
                    new Collection<JobDetailDamage>(jobDetailDamages.Where(n => n.JobDetailId == jobDetail.Id).ToList());
                jobDetail.Actions =
                    new Collection<JobDetailAction>(actions.Where(a => a.JobDetailId == jobDetail.Id).ToList());
            }

            return jobDetails;
        }

        protected override void SaveNew(JobDetail jobDetail)
        {
            jobDetail.Id = dapperProxy.WithStoredProcedure("JobDetail_Insert")
                .AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32)
                .AddParameter("OriginalDespatchQty", jobDetail.OriginalDespatchQty, DbType.Int32)
                .AddParameter("ProdDesc", jobDetail.ProdDesc, DbType.String)
                .AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32)
                .AddParameter("DeliveredQty", string.IsNullOrWhiteSpace(jobDetail.DeliveredQty) ? 0m : decimal.Parse(jobDetail.DeliveredQty), DbType.Decimal) //TODO should this be int not decimal?
                .AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32)
                .AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String)
                .AddParameter("PHProductCode", jobDetail.PhProductCode, DbType.Int32)           
                .AddParameter("PHProductType", jobDetail.PhProductType, DbType.String)
                .AddParameter("PackSize", jobDetail.PackSize, DbType.String)
                .AddParameter("SingleOrOuter", jobDetail.SingleOrOuter, DbType.String)
                .AddParameter("SSCCBarcode", jobDetail.SsccBarcode, DbType.String)
                .AddParameter("SubOuterDamageTotal", jobDetail.SubOuterDamageTotal, DbType.Int32)
                .AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Double)
                .AddParameter("NetPrice", jobDetail.NetPrice, DbType.Double)
                .AddParameter("JobId", jobDetail.JobId, DbType.Int32)
                .AddParameter("JobDetailStatusId", jobDetail.JobDetailStatusId, DbType.Int32)
                .AddParameter("CreatedBy", jobDetail.CreatedBy, DbType.String)
                .AddParameter("DateCreated", jobDetail.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedBy", jobDetail.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", jobDetail.DateUpdated, DbType.DateTime)
                .AddParameter("IsDeleted", jobDetail.JobDetailStatusId == (int)JobDetailStatus.Res, DbType.Boolean)
                .Query<int>().FirstOrDefault();
        }

        protected override void UpdateExisting(JobDetail jobDetail)
        {
            var credit = jobDetail.CreditValueForThreshold();

            dapperProxy.WithStoredProcedure("JobDetail_Update")
                .AddParameter("Id", jobDetail.Id, DbType.Int32)
                .AddParameter("OriginalDespatchQty", jobDetail.OriginalDespatchQty, DbType.Int32)
                .AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32)
                .AddParameter("DeliveredQty", string.IsNullOrWhiteSpace(jobDetail.DeliveredQty) ? 0m : decimal.Parse(jobDetail.DeliveredQty), DbType.Decimal)
                .AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32)
                .AddParameter("JobDetailStatusId", jobDetail.JobDetailStatusId, DbType.Int32)
                .AddParameter("IsDeleted", jobDetail.IsDeleted, DbType.String)
                .AddParameter("UpdatedBy", jobDetail.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", jobDetail.DateUpdated, DbType.DateTime)
                .AddParameter("LineStatus", jobDetail.LineStatus, DbType.String)
                .AddParameter("SubOuterDamageQty", jobDetail.SubOuterDamageTotal, DbType.Int16)
                .Execute();
        }


        public void DeleteJobDetailById(int id)
        {
            this.JobDetailDeleteDamageReasonsByJobDetailId(id);

            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailDeleteById)
                .AddParameter("JobDetailId", id, DbType.Int32)
                .Execute();          
        }

        private void JobDetailDeleteDamageReasonsByJobDetailId(int jobDetailId)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailDeleteDamageReasonsByJobDetailId)
                 .AddParameter("JobDetailId", jobDetailId, DbType.Int32)
                .Execute();
        }

        public void CreditLines(DataTable creditLinesTable)
        {
            dapperProxy.WithStoredProcedure("JobDetail_CreditLines")
                .AddParameter("CreditLines", creditLinesTable, DbType.Object)
                .Execute();
        }
    }
}
