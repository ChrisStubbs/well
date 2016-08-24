namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;

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

        public IEnumerable<JobDetail>  GetByJobId(int id)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailGetByJobId)
                .AddParameter("JobId", id, DbType.Int32)
                .Query<JobDetail>();
        }


        

        public JobDetail GetByJobLine(int jobId, int lineNumber)
        {
            throw new System.NotImplementedException();
        }

        public JobDetail JobDetailGetByBarcodeAndProdDesc(string barcode, int jobId)
        {
            var jobDetail =
               dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailGetByBarcodeAndProdDesc)
                   .AddParameter("Barcode", barcode, DbType.String)
                   .AddParameter("JobId", jobId, DbType.Int32)
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
                .AddParameter("JobDetailStatusId", jobDetail.JobDetailStatusId, DbType.Int32)
                .AddParameter("IsDeleted", jobDetail.IsDeleted = jobDetail.JobDetailStatusId == (int)JobDetailStatus.Res, DbType.Boolean)
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

        public void CreateOrUpdateJobDetailDamage(JobDetailDamage jobDetailDamage)
        {
            var id = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailDamageCreateOrUpdate)
                .AddParameter("Id", jobDetailDamage.Id, DbType.Int32)
                .AddParameter("JobDetailId", jobDetailDamage.JobDetailId, DbType.Int32)
                .AddParameter("Qty", jobDetailDamage.Qty, DbType.Decimal)
                .AddParameter("DamageReasonsId", (int)jobDetailDamage.Reason, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .Query<int>().FirstOrDefault();

            if (jobDetailDamage.IsTransient())
            {
                jobDetailDamage.Id = id;
            }
        }

        public void DeleteJobDetailById(int id)
        {
            JobDetailArttributesDeleteByJobDetailId(id);
            JobDetailDeleteDamageReasonsByJobDetailId(id);

            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailDeleteById)
                .AddParameter("JobDetailId", id, DbType.Int32).Execute();          
        }

        private void JobDetailArttributesDeleteByJobDetailId(int jobDetailId)
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
