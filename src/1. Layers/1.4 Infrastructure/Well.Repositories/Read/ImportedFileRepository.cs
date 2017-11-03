namespace PH.Well.Repositories.Read
{
    using System;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;

    public class ImportedFileRepository : IImportedFileRepository
    {
        private readonly ILogger logger;
        private readonly IDapperProxy dapperProxy;

        public ImportedFileRepository(ILogger logger, IDapperProxy dapperProxy)
        {
            this.logger = logger;
            this.dapperProxy = dapperProxy;
        }

        public bool HasFileAlreadyBeenImported(string importFileName)
        {
            var noOfFiles = dapperProxy.WithStoredProcedure(StoredProcedures.ImportedFileNameCount)
                                   .AddParameter("FileName", importFileName, DbType.String).Query<int>().Single();

            return noOfFiles > 0;
        }
    }
}