using PH.Shared.Well.Data.EF;

namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Newtonsoft.Json;
    using System.Linq;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using Dapper;

    public class ExceptionEventRepository : DapperRepository<ExceptionEvent, int>, IExceptionEventRepository
    {
        
        public ExceptionEventRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider)
            : base(logger, dapperProxy, userNameProvider)
        {
        }

        public void InsertCreditEventTransaction(CreditTransaction creditTransaction)
        {
            SerializeToJsonAndInsertEvent(creditTransaction, EventAction.Credit, DateTime.Now, creditTransaction.JobId.ToString());
        }

        public void MarkEventAsProcessed(int eventId)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.MarkEventAsProcessed)
                .AddParameter("EventId", eventId, DbType.Int32)
                .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        public void Delete(int id)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.ExceptionEventDelete)
                .AddParameter("Id", id, DbType.Int32)
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

        public void InsertGrnEvent(GrnEvent grnEvent, DateTime dateCanBeProcessed, string jobId)
        {
            SerializeToJsonAndInsertEvent(grnEvent, EventAction.Grn, dateCanBeProcessed, jobId);
        }

        public bool GrnEventCreatedForJob(string jobId)
        {
            return ExceptionEventCreatedForSourceId(jobId, EventAction.Grn);
        }

        public bool PodEventCreatedForJob(string jobId)
        {
            return ExceptionEventCreatedForSourceId(jobId, EventAction.Pod);
        }

        public void InsertPodTransaction(PodTransaction podTransaction)
        {
            SerializeToJsonAndInsertEvent(podTransaction, EventAction.PodTransaction, DateTime.Now, podTransaction.JobId.ToString());
        }

        public void InsertPodEvent(PodEvent podEvent, string jobId, DateTime dateCanBeProcessed)
        {
            SerializeToJsonAndInsertEvent(podEvent, EventAction.Pod, dateCanBeProcessed, jobId);
        }


        public void InsertAmendmentTransaction(AmendmentTransaction amendmentEvent)
        {
            SerializeToJsonAndInsertEvent(amendmentEvent, EventAction.Amendment, DateTime.Now);
        }

        public void InsertGlobalUpliftEvent(GlobalUpliftEvent globalUpliftEvent, string sourceId = null)
        {
            SerializeToJsonAndInsertEvent(globalUpliftEvent, EventAction.GlobalUplift, DateTime.Now, sourceId);
        }

        public bool GlobalUpliftEventCreatedForJob(string jobId)
        {
            return ExceptionEventCreatedForSourceId(jobId, EventAction.GlobalUplift);
        }

        private void SerializeToJsonAndInsertEvent(object eventData, EventAction action, DateTime dateCanBeProcessed,
            string sourceId = null)
        {
            string eventDataJson = null;
            if (eventData != null)
            {
                eventDataJson = JsonConvert.SerializeObject(eventData);
            }

            InsertEvent(eventDataJson, action, dateCanBeProcessed, sourceId);

        }

        private void InsertEvent(string stringEvent, EventAction action, DateTime dateCanBeProcessed, string sourceId)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.EventInsert)
                .AddParameter("Event", stringEvent, DbType.String)
                .AddParameter("ExceptionActionId", action, DbType.Int32)
                .AddParameter("DateCanBeProcessed", dateCanBeProcessed, DbType.DateTime)
                .AddParameter("SourceId", sourceId, DbType.String)
                .AddParameter("CreatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        private ExceptionEvent GetExceptionEventByActionAndSourceId(string sourceId, EventAction eventAction)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.EventGetBySourceId)
                   .AddParameter("ExceptionActionId", (int)eventAction, DbType.Int32)
                   .AddParameter("SourceId", sourceId, DbType.String).Query<ExceptionEvent>()
                   .FirstOrDefault();
        }

        private bool ExceptionEventCreatedForSourceId(string sourceId, EventAction eventAction)
        {
            return GetExceptionEventByActionAndSourceId(sourceId, eventAction) != null;
        }
    }
}