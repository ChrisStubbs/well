namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Extensions;
    using Contracts;

    public class PostImportRepository : IPostImportRepository
    {
        private readonly IDapperProxy dapperProxy;

        public PostImportRepository(IDapperProxy dapperProxy)
        {
            this.dapperProxy = dapperProxy;
        }

        public void PostImportUpdate()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.PostImportUpdate)
                .Execute();
        }

        public void PostTranSendImportForTobacco(IEnumerable<int> jobIds)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailTobaccoUpdate)
                .AddParameter("JobIds", jobIds.ToList().ToIntDataTables("JobIds"), DbType.Object)
                .Execute();
        }

        public void PostTranSendImport(IEnumerable<int> jobIds)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.LineItemActionInsert)
                .AddParameter("JobIds", jobIds.ToList().ToIntDataTables("JobIds"), DbType.Object)
                .Execute();
        }

        public void PostTranSendImportShortsTba(IEnumerable<int> jobIds)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobUpdateShortsTba)
                .AddParameter("Ids", jobIds.ToList().ToIntDataTables("Ids"), DbType.Object)
                .Execute();
        }


    }
}
