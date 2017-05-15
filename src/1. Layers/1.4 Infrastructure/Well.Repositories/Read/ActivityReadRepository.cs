namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class ActivityReadRepository : IActivityReadRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;
        private readonly ILineItemSearchReadRepository lineItemSearchReadRepository;

        public ActivityReadRepository(ILogger logger, IDapperReadProxy dapperReadProxy, 
            ILineItemSearchReadRepository lineItemSearchReadRepository, ILineItemActionReadRepository lineItemActionReadRepository)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
            this.lineItemSearchReadRepository = lineItemSearchReadRepository;
        }

        public Activity GetById(int id)
        {
            var activity = dapperReadProxy.WithStoredProcedure(StoredProcedures.ActivityGetById)
                   .AddParameter("Id", id, DbType.Int32)
                   .Query<Activity>().FirstOrDefault();

            if (activity != null)
            {
                var lineItems = this.lineItemSearchReadRepository.GetLineItemByActivityId(id).ToList();

                activity.LineItems = lineItems;
            }

            return activity;
        }
    }
}
