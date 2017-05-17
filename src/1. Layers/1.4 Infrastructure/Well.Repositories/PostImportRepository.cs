namespace PH.Well.Repositories
{
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

        public void PostTranSendImport()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.LineItemActionInsert)
                .Execute();
        }
    }
}
