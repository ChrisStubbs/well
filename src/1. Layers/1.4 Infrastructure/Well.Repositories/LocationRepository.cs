namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class LocationRepository : ILocationRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;

        public LocationRepository(ILogger logger, IDapperReadProxy dapperReadProxy)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
        }

        public SingleLocation GetSingleLocation(int? locationId, string accountNumber = null, int? branchId = default(int?))
        {
            SingleLocation result = null;

            dapperReadProxy.WithStoredProcedure(StoredProcedures.GetSingleLocation)
                    .AddParameter("locationId", locationId, DbType.Int32)
                    .AddParameter("AccountNumber", accountNumber, DbType.String)
                    .AddParameter("BranchId", branchId, DbType.Int32)
                    .QueryMultiple(p =>
                    {
                        result = p.Read<SingleLocation>().First();

                        result.Details = p.Read<SingleLocationItems>().ToList();

                        return result;
                    });

            return result;
        }

        public IList<Location> GetLocation(int branchId)
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.GetLocations)
                    .AddParameter("BranchId", branchId, DbType.Int32)
                    .Query<Location>()
                    .OrderBy(p => p.PrimaryAccountNumber)
                    .ToList();
        }
    }
}
