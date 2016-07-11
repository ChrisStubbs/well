namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class CleanDeliveryRepository : DapperRepository<CleanDelivery, int> , ICleanDeliveryRepository
    {
        public CleanDeliveryRepository(ILogger logger, IWellDapperProxy dapperProxy
            )
            : base(logger, dapperProxy)
        {
        }

        public IEnumerable<CleanDelivery> GetCleanDeliveries()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.JobGetCleanDeliveries).Query<CleanDelivery>();
        }
    }
}
