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
        public ExceptionEventRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider) 
            : base(logger, dapperProxy, userNameProvider)
        {
        }

        public void InsertCreditEventTransaction(CreditTransaction creditTransaction)
        {
            var creditEventTransactionJson = JsonConvert.SerializeObject(creditTransaction);

            this.dapperProxy.WithStoredProcedure(StoredProcedures.EventInsert)
                .AddParameter("Event", creditEventTransactionJson, DbType.String)
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

        public void RemovedPendingCredit(int jobId)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.RemovePendingCredit)
                .AddParameter("jobId", jobId, DbType.Int32)
                .Execute();
        }

        public void InsertGrnEvent(GrnEvent grnEvent,DateTime dateCanBeProcessed)
        {
            // GrnEvent.Id is the same as related job id
            InsertEvent(EventAction.Grn, grnEvent, dateCanBeProcessed, grnEvent.Id.ToString());
        }

        public void InsertPodTransaction(PodTransaction podTransaction)
        {
            var podEventJson = JsonConvert.SerializeObject(podTransaction);

            this.dapperProxy.WithStoredProcedure(StoredProcedures.EventInsert)
                .AddParameter("Event", podEventJson, DbType.String)
                .AddParameter("ExceptionActionId", EventAction.PodTransaction, DbType.Int32)
                .AddParameter("DateCanBeProcessed", DateTime.Now, DbType.DateTime)
                .AddParameter("CreatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                .Execute();
        }
        
        public void InsertPodEvent(PodEvent podEvent)
        {
            // PodEvent.Id is the same as related job id
            InsertEvent(EventAction.Pod, podEvent, entityId: podEvent.Id.ToString());
        }

        public void InsertEvent(EventAction action, object eventData, DateTime? dateCanBeProcessed = null,string entityId = null)
        {
            var eventDataJson = JsonConvert.SerializeObject(eventData);

            this.dapperProxy.WithStoredProcedure(StoredProcedures.EventInsert)
                .AddParameter("Event", eventDataJson, DbType.String)
                .AddParameter("ExceptionActionId", action, DbType.Int32)
                .AddParameter("DateCanBeProcessed", dateCanBeProcessed ?? DateTime.Now, DbType.DateTime)
                .AddParameter("CreatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                .AddParameter("EntityId", entityId, DbType.String, size: 50)
                .Execute();
        }

        public IEnumerable<ExceptionEvent> GetEventsByEntityId(string entityId, EventAction action)
        {
            if (string.IsNullOrWhiteSpace(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            return this.dapperProxy.WithStoredProcedure(StoredProcedures.EventGetByEntityId)
                .AddParameter("EntityId", entityId, DbType.String, size: 50)
                .AddParameter("ExceptionActionId", (int) action, DbType.Int32)
                .Query<ExceptionEvent>();
        }

        public void InsertAmendmentTransaction(AmendmentTransaction amendmentEvent)
        {
            var amendmentEventJson = JsonConvert.SerializeObject(amendmentEvent);

            this.dapperProxy.WithStoredProcedure(StoredProcedures.EventInsert)
                .AddParameter("Event", amendmentEventJson, DbType.String)
                .AddParameter("ExceptionActionId", EventAction.Amendment, DbType.Int32)
                .AddParameter("DateCanBeProcessed", DateTime.Now, DbType.DateTime)
                .AddParameter("CreatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                .Execute();
        }
    }
}