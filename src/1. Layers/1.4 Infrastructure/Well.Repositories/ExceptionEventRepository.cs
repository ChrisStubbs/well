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
            var creditEventTransactionJson = JsonConvert.SerializeObject(creditTransaction);

            this.PrepareStoredProcedure(creditEventTransactionJson, EventAction.Credit, DateTime.Now)
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

        public void InsertGrnEvent(GrnEvent grnEvent, DateTime dateCanBeProcessed)
        {
            var grnEventJson = JsonConvert.SerializeObject(grnEvent);

            this.PrepareStoredProcedure(grnEventJson, EventAction.Grn, dateCanBeProcessed)
            .Execute();
        }

        public void InsertPodTransaction(PodTransaction podTransaction)
        {
            var podEventJson = JsonConvert.SerializeObject(podTransaction);

            this.PrepareStoredProcedure(podEventJson, EventAction.PodTransaction, DateTime.Now)
            .Execute();
        }

        public void InsertPodEvent(PodEvent podEvent)
        {
            var podEventJson = JsonConvert.SerializeObject(podEvent);

            this.PrepareStoredProcedure(podEventJson, EventAction.Pod, DateTime.Now.Date.AddDays(1))
            .Execute();
        }

        public void InsertEvent(EventAction action, object eventData, DateTime? dateCanBeProcessed = null)
        {
            var eventDataJson = JsonConvert.SerializeObject(eventData);

            this.PrepareStoredProcedure(eventDataJson, action, dateCanBeProcessed ?? DateTime.Now)
            .Execute();
        }

        public Task InsertAmendmentTransactionAsync(IList<AmendmentTransaction> amendmentEvent)
        {
            var data = amendmentEvent
                .Select(p => new
                {
                    @Event = JsonConvert.SerializeObject(amendmentEvent),
                    ExceptionActionId = (int)EventAction.Amendment,
                    DateCanBeProcessed = DateTime.Now,
                    CreatedBy = this.CurrentUser,
                    DateCreated = DateTime.Now,
                    UpdatedBy = this.CurrentUser,
                    DateUpdated = DateTime.Now
                })
                .ToList()
                .ToDataTables();

            var par = new DynamicParameters();

            par.Add("Data", data, DbType.Object);

            return this.dapperProxy.ExecuteAsync(par, StoredProcedures.EventInsertBulk);
        }

        public void InsertAmendmentTransaction(AmendmentTransaction amendmentEvent)
        {
            var amendmentEventJson = JsonConvert.SerializeObject(amendmentEvent);

            this.PrepareStoredProcedure(amendmentEventJson, EventAction.Amendment, DateTime.Now)
            .Execute();
        }

        private IDapperProxy PrepareStoredProcedure(string stringEvent, EventAction action, DateTime dateCanBeProcessed)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.EventInsert)
                .AddParameter("Event", stringEvent, DbType.String)
                .AddParameter("ExceptionActionId", action, DbType.Int32)
                .AddParameter("DateCanBeProcessed", dateCanBeProcessed, DbType.DateTime)
                .AddParameter("CreatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime);
        }
    }
}