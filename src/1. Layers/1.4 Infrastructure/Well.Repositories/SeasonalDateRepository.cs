namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;

    using Common.Contracts;
    using PH.Well.Domain;
    using Contracts;
    
    public class SeasonalDateRepository : DapperRepository<SeasonalDate, int>, ISeasonalDateRepository
    {
        public SeasonalDateRepository(ILogger logger, IDapperProxy dapperProxy) : base(logger, dapperProxy)
        {
        }

        public IEnumerable<SeasonalDate> GetAll()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesGetAll).Query<SeasonalDate>();
        }

        public void Delete(int id)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesDelete)
                .AddParameter("Id", id, DbType.Int32).Execute();
        }
    }
}