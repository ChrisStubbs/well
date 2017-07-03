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

        public void PostTranSendImportForTobacco()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailTobaccoUpdate)
                .Execute();
        }

        public void PostTranSendImport()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.LineItemActionInsert)
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
