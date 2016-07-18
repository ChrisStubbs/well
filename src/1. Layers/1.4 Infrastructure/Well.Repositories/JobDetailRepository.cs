namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
    public class JobDetailRepository : DapperRepository<JobDetail, int>, IJobDetailRepository
    {


        public JobDetailRepository(ILogger logger, IWellDapperProxy dapperProxy) : base(logger, dapperProxy)
        {}


        public JobDetail GetById(int id)
        {
            var jobDetail =
               dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailGetById)
                   .AddParameter("Id", id, DbType.Int32)
                   .Query<JobDetail>()
                   .FirstOrDefault();

            return jobDetail;
        }

        public JobDetail JobDetailCreateOrUpdate(JobDetail jobDetail)
        {
            var id = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailCreateOrUpdate)
                .AddParameter("Id", jobDetail.Id, DbType.Int32)
                .AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .AddParameter("Barcode", jobDetail.BarCode, DbType.Int32)
                .AddParameter("OriginalDespatchQty", jobDetail.OriginalDispatchQty, DbType.Decimal)
                .AddParameter("ProdDesc", jobDetail.ProdDesc, DbType.String)
                .AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32)
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
                .Query<int>().FirstOrDefault();

            return this.GetById(id);

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

        public JobDetail GetByBarcodeLineNumberAndJobId(int lineNumber, string barcode, int jobId)
        {
            var jobDetail =
               dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailGetByBarcodeLineNumberAndJobId)
                   .AddParameter("LineNumber", lineNumber, DbType.Int32)
                   .AddParameter("Barcode", barcode, DbType.String)
                   .AddParameter("JobId", jobId, DbType.Int32)
                   .Query<JobDetail>()
                   .FirstOrDefault();

            return jobDetail;
        }

        public void JobDetailDamageCreateOrUpdate(JobDetailDamage jobDetailDamage)
        {
            var id = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailCreateOrUpdate)
                .AddParameter("Id", jobDetailDamage.Id, DbType.Int32)
                .AddParameter("JobDetailId", jobDetailDamage.JobDetailId, DbType.Int32)
                .AddParameter("Qty", jobDetailDamage.Qty, DbType.Int32)
                .AddParameter("DamageReasonsId", jobDetailDamage.DamageReasonId, DbType.Int32)
                .AddParameter("DamageReasonCategoryId", jobDetailDamage.ReasonCategoryId, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .Query<int>().FirstOrDefault();
        }

    }


}
