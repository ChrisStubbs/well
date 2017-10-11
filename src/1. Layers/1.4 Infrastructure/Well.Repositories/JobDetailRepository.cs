namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain;

    public class JobDetailRepository : DapperRepository<JobDetail, int>, IJobDetailRepository
    {
        private const string UnknownLineDeliveryStatus = "Unknown";

        public JobDetailRepository(ILogger logger, IWellDapperProxy dapperProxy, IUserNameProvider userNameProvider) :
            base(logger, dapperProxy, userNameProvider)
        { }

        public JobDetail GetById(int id)
        {
            return Get(id, null, null).FirstOrDefault();
        }

        public IEnumerable<JobDetail> GetByJobId(int jobId)
        {
            return Get(null, jobId, null);
        }

        public JobDetail GetByJobLine(int jobId, int lineNumber)
        {
            return Get(null, jobId, lineNumber).FirstOrDefault();
        }

        private IEnumerable<JobDetail> Get(int? id, int? jobId, int? lineNumber)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailGet)
                .AddParameter("Id", id, DbType.Int32)
                .AddParameter("JobId", jobId, DbType.Int32)
                .AddParameter("LineNumber", lineNumber, DbType.Int32)
                .QueryMultiples(GetFromGrid);
        }

        private List<JobDetail> GetFromGrid(SqlMapper.GridReader grid)
        {
            var jobDetails = grid.Read<JobDetail>().ToList();
            var jobDetailDamages = grid.Read<JobDetailDamage>().ToList();
            foreach (var jobDetail in jobDetails)
            {
                jobDetail.JobDetailDamages =
                    new List<JobDetailDamage>(jobDetailDamages.Where(n => n.JobDetailId == jobDetail.Id));
            }

            return jobDetails;
        }

        protected override void SaveNew(JobDetail jobDetail)
        {
            jobDetail.Id = dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailInsert)
                .AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32)
                .AddParameter("OriginalDespatchQty", jobDetail.OriginalDespatchQty, DbType.Int32)
                .AddParameter("ProdDesc", jobDetail.ProdDesc, DbType.String)
                .AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32)
                .AddParameter("DeliveredQty", jobDetail.DeliveredQty, DbType.Int32)
                .AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32)
                .AddParameter("JobDetailReasonId", jobDetail.JobDetailReason, DbType.Int32)
                .AddParameter("JobDetailSourceId", jobDetail.JobDetailSource, DbType.Int32)
                .AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String)
                .AddParameter("PHProductCode", jobDetail.PhProductCode, DbType.String)
                .AddParameter("PHProductType", jobDetail.PhProductType, DbType.String)
                .AddParameter("PackSize", jobDetail.PackSize, DbType.String)
                .AddParameter("SingleOrOuter", jobDetail.SingleOrOuter, DbType.String)
                .AddParameter("SSCCBarcode", jobDetail.SSCCBarcode, DbType.String)
                .AddParameter("SubOuterDamageTotal", jobDetail.SubOuterDamageTotal, DbType.Int32)
                .AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Double)
                .AddParameter("NetPrice", jobDetail.NetPrice, DbType.Double)
                .AddParameter("JobId", jobDetail.JobId, DbType.Int32)
                .AddParameter("ShortsStatus", jobDetail.ShortsStatus, DbType.Int32)
                .AddParameter("LineDeliveryStatus", jobDetail.LineDeliveryStatus ?? UnknownLineDeliveryStatus, DbType.String)
                .AddParameter("IsHighValue", jobDetail.IsHighValue, DbType.Boolean)
                .AddParameter("CreatedBy", jobDetail.CreatedBy, DbType.String)
                .AddParameter("DateCreated", jobDetail.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedBy", jobDetail.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", jobDetail.DateUpdated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }

        protected override void UpdateExisting(JobDetail jobDetail)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailUpdate)
                .AddParameter("Id", jobDetail.Id, DbType.Int32)
                .AddParameter("DeliveredQty", jobDetail.DeliveredQty, DbType.Int32)
                .AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32)
                .AddParameter("JobDetailReasonId", jobDetail.JobDetailReasonId, DbType.Int32)
                .AddParameter("JobDetailSourceId", jobDetail.JobDetailSourceId, DbType.Int32)
                .AddParameter("ShortsStatus", jobDetail.ShortsStatus, DbType.Int32)
                .AddParameter("ShortsActionId", jobDetail.ShortsActionId, DbType.Int32)
                .AddParameter("LineDeliveryStatus", jobDetail.LineDeliveryStatus ?? UnknownLineDeliveryStatus, DbType.String)
                .AddParameter("SubOuterDamageQty", jobDetail.SubOuterDamageTotal, DbType.Int16)
                .AddParameter("ProductCode", jobDetail.PhProductCode, DbType.String)
                .AddParameter("ProductDescription", jobDetail.ProdDesc, DbType.String)
                .AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32)
                .AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String)
                .AddParameter("ProductType", jobDetail.PhProductType, DbType.String)
                .AddParameter("PackSize", jobDetail.PackSize, DbType.String)
                .AddParameter("SingleOrOuter", jobDetail.SingleOrOuter, DbType.String)
                .AddParameter("Barcode", jobDetail.SSCCBarcode, DbType.String) // tobacco bag barcode
                .AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Decimal)
                .AddParameter("UpdatedBy", jobDetail.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", jobDetail.DateUpdated, DbType.DateTime)
                .AddParameter("OriginalDespatchQty", jobDetail.OriginalDespatchQty, DbType.Int32)
                .AddParameter("NetPrice", jobDetail.NetPrice, DbType.Decimal)
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
    }
}
