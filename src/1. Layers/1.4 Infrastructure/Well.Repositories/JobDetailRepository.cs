namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain;

    public class JobDetailRepository : DapperRepository<JobDetail, int>, IJobDetailRepository
    {

        public JobDetailRepository(ILogger logger, IWellDapperProxy dapperProxy) : base(logger, dapperProxy)
        {}

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
            var jobDetailAttributes = grid.Read<Attribute>().ToList();

            foreach (var jobDetail in jobDetails)
            {
                jobDetail.JobDetailDamages =
                    new Collection<JobDetailDamage>(jobDetailDamages.Where(n => n.JobDetailId == jobDetail.Id).ToList());

                jobDetail.EntityAttributes =
                    new Collection<Attribute>(jobDetailAttributes.Where(a => a.AttributeId == jobDetail.Id).ToList());
            }

            return jobDetails;
        }

        protected override void SaveNew(JobDetail jobDetail)
        {
            

            jobDetail.Id = dapperProxy.WithStoredProcedure("JobDetail_Insert")
                .AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32)
                .AddParameter("Barcode", jobDetail.BarCode, DbType.Int32)
                .AddParameter("OriginalDespatchQty", jobDetail.OriginalDispatchQty, DbType.Decimal)
                .AddParameter("ProdDesc", jobDetail.ProdDesc, DbType.String)
                .AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32)
                .AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32)
                .AddParameter("SkuWeight", jobDetail.SkuWeight, DbType.Decimal)
                .AddParameter("SkuCube", jobDetail.SkuCube, DbType.Decimal)
                .AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String)
                .AddParameter("TextField1", jobDetail.TextField1, DbType.String)
                .AddParameter("TextField2", jobDetail.TextField2, DbType.String)
                .AddParameter("TextField3", jobDetail.TextField3, DbType.String)
                .AddParameter("TextField4", jobDetail.TextField4, DbType.String)
                .AddParameter("TextField5", jobDetail.TextField5, DbType.String)
                .AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Double)
                .AddParameter("JobId", jobDetail.JobId, DbType.Int32)
                .AddParameter("JobDetailStatusId", jobDetail.JobDetailStatusId, DbType.Int32)
                .AddParameter("CreatedBy", jobDetail.CreatedBy, DbType.String)
                .AddParameter("DateCreated", jobDetail.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedBy", jobDetail.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", jobDetail.DateUpdated, DbType.DateTime)
                .AddParameter("IsDeleted", jobDetail.JobDetailStatusId == (int)JobDetailStatus.Res, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }

        protected override void UpdateExisting(JobDetail jobDetail)
        {
            dapperProxy.WithStoredProcedure("JobDetail_Update")
                .AddParameter("Id", jobDetail.Id, DbType.Int32)
                .AddParameter("OriginalDespatchQty", jobDetail.OriginalDispatchQty, DbType.Decimal)
                .AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32)
                .AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32)
                .AddParameter("JobDetailStatusId", jobDetail.JobDetailStatusId, DbType.Int32)
                .AddParameter("IsDeleted", jobDetail.IsDeleted, DbType.String)
                .AddParameter("UpdatedBy", jobDetail.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", jobDetail.DateUpdated, DbType.DateTime)
                .Execute();
        }

        public void AddJobDetailAttributes(Attribute attribute)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailAttributeCreateOrUpdate)
                .AddParameter("Id", attribute.Id, DbType.Int32)
                .AddParameter("Code", attribute.Code, DbType.String)
                .AddParameter("Value", attribute.Value1, DbType.String)
                .AddParameter("JobDetailId", attribute.AttributeId, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String).Query<int>();
        }

        public void DeleteJobDetailById(int id)
        {
            JobDetailAttributesDeleteByJobDetailId(id);
            JobDetailDeleteDamageReasonsByJobDetailId(id);

            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailDeleteById)
                .AddParameter("JobDetailId", id, DbType.Int32).Execute();          
        }

        private void JobDetailAttributesDeleteByJobDetailId(int jobDetailId)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailArttributesDeleteByJobDetailId)
                .AddParameter("JobDetailId", jobDetailId, DbType.Int32).Execute();
        }

        private void JobDetailDeleteDamageReasonsByJobDetailId(int id)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailDeleteDamageReasonsByJobDetailId)
                .AddParameter("JobDetailId", id, DbType.Int32).Execute();         
        }
    }
}
