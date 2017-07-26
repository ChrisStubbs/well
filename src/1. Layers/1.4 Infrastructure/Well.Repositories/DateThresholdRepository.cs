using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PH.Well.Common.Contracts;
using PH.Well.Domain;
using PH.Well.Repositories.Contracts;

namespace PH.Well.Repositories
{
    public class DateThresholdRepository : DapperRepository<DateThreshold, int>, IDateThresholdRepository
    {
        public DateThresholdRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider) : base(logger, dapperProxy, userNameProvider)
        {
        }

        public void Delete(int branchId)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.CleanPreferenceDelete)
                .AddParameter("branchId", branchId, System.Data.DbType.Int32).Execute();
        }

        private IDapperProxy LoadStoreProcedure()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.DateThreshold);
        }

        public Task<IEnumerable<DateThreshold>> GetAsync()
        {
            return this.dapperProxy.QueryAsync<DateThreshold>(null, StoredProcedures.DateThreshold);
        }

        public IList<DateThreshold> Get()
        {
            return this.LoadStoreProcedure().Query<DateThreshold>().ToList();
        }
    }
}
