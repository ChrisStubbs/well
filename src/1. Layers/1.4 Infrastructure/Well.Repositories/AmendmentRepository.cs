namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
    using Contracts;
    using Dapper;
    using Domain.ValueObjects;

    public class AmendmentRepository : IAmendmentRepository
    {
        private readonly ILogger logger;
        private readonly IWellDapperProxy dapperProxy;


        public AmendmentRepository(ILogger logger, IWellDapperProxy dapperProxy)
        {
            this.logger = logger;
            this.dapperProxy = dapperProxy;
        }

        public IEnumerable<Amendment> GetAmendments(IEnumerable<int> jobIds)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.GetAmendments)
                .AddParameter("Ids", jobIds.ToList().ToIntDataTables("Ids"), DbType.Object)
                .QueryMultiple(GetFromGrid);

        }

        private List<Amendment> GetFromGrid(SqlMapper.GridReader grid)
        {
            var amendments = grid.Read<Amendment>().ToList();
            var amendmentLines = grid.Read<AmendmentLine>().ToList();

            foreach (var amendment in amendments)
              {
                  amendment.AmendmentLines =
                       new List<AmendmentLine>(amendmentLines.Where(a => a.JobId == amendment.JobId));
               }
            
            return amendments;
        }

    }
}
