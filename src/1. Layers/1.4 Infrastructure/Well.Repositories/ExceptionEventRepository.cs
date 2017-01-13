namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Newtonsoft.Json;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    public class ExceptionEventRepository : DapperRepository<ExceptionEvent, int>, IExceptionEventRepository
    {
        public ExceptionEventRepository(ILogger logger, IDapperProxy dapperProxy) : base(logger, dapperProxy)
        {
        }

        public void InsertCreditEvent(CreditEvent creditEvent)
        {
            var creditEventJson = JsonConvert.SerializeObject(creditEvent);

            this.dapperProxy.WithStoredProcedure(StoredProcedures.EventInsert)
                .AddParameter("Event", creditEventJson, DbType.String, size: 2500)
                .AddParameter("ExceptionActionId", EventAction.Credit, DbType.Int32)
                .AddParameter("DateCanBeProcessed", DateTime.Now, DbType.DateTime)
                .AddParameter("CreatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        public void MarkEventAsProcessed(int eventId)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.MarkEventAsProcessed)
                .AddParameter("EventId", eventId, DbType.Int32)
                .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        public IEnumerable<ExceptionEvent> GetAllUnprocessed()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.EventGetUnprocessed).Query<ExceptionEvent>();
        }

        public void RemovedPendingCredit(string invoiceNumber)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.RemovePendingCredit)
                .AddParameter("invoiceNumber", invoiceNumber, DbType.String)
                .Execute();
        }
    }
}