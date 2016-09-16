namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
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
    }
}