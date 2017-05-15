namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class LineItemActionReadRepository : ILineItemActionReadRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;

        public LineItemActionReadRepository(ILogger logger, IDapperReadProxy dapperReadProxy)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
        }

        public IEnumerable<LineItemAction> GetLineItemActionByLineItemId(int id)
        {
            IEnumerable<LineItemAction> lineItemActions = new List<LineItemAction>();
            this.dapperReadProxy.WithStoredProcedure(StoredProcedures.LineItemActionGetByLineItemId)
                .AddParameter("Id", id, DbType.Int32)
                .Query<int>();

            return lineItemActions;
        }
    }
}
