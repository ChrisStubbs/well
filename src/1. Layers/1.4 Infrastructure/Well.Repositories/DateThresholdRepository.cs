using System;
using System.Collections.Generic;
using System.Linq;
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

        public IList<DateThreshold> Get()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.DateThreshold).Query<DateThreshold>().ToList();
        }

        protected override void UpdateExisting(DateThreshold entity)
        {
            throw new NotImplementedException();
        }
    }
}
