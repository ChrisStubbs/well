﻿namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
    using Contracts;
    using Dapper;
    using Domain;

    public class LineItemSearchReadRepository : ILineItemSearchReadRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;

        public LineItemSearchReadRepository(ILogger logger, IDapperReadProxy dapperReadProxy)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
        }

        public LineItem GetById(int id)
        {
            return GetLineItemByIds(new[] {id}).FirstOrDefault();
        }

        public IEnumerable<LineItem> GetLineItemByIds(IEnumerable<int> ids)
        {
            IEnumerable<LineItem> lineItems = new List<LineItem>();
            this.dapperReadProxy.WithStoredProcedure(StoredProcedures.LineItemGetByIds)
                .AddParameter("Ids", ids.ToList().ToIntDataTables("Ids"), DbType.Object)
                .QueryMultiple(x => lineItems = GetLineItemsByGrid(x));

            return lineItems;
        }

        private IEnumerable<LineItem> GetLineItemsByGrid(SqlMapper.GridReader gridReader)
        {
            var lineItems = gridReader.Read<LineItem>().ToList();
            var lineItemActions = gridReader.Read<LineItemAction>().ToList();
            var comments = gridReader.Read<LineItemActionComment>().ToList();

            foreach (var lineItem in lineItems)
            {

                lineItem.LineItemActions = lineItemActions.Where(x => x.LineItemId == lineItem.Id).ToList();

                foreach (var lineItemAction in lineItem.LineItemActions.ToArray())
                {
                    lineItemAction.Comments = comments.Where(x => x.LineItemActionId == lineItemAction.Id);
                }
            }
           

            return lineItems;
        }


        //TODO: Change this so that it gets the Line Items from The GetLineItemsByIds method Sproc!!
        public IEnumerable<LineItem> GetLineItemByActivityId(int id)
        {
            IEnumerable<LineItem> lineItems = new List<LineItem>();
            this.dapperReadProxy.WithStoredProcedure(StoredProcedures.LineItemGetByActivityId)
                .AddParameter("Id", id, DbType.Int32)
                .QueryMultiple(x => lineItems = GetLineItemsByGrid(x));

            return lineItems;
        }

    }
}
