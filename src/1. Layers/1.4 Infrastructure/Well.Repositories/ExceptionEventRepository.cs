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
        private readonly WellEntities wellEntities;

        public ExceptionEventRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider, WellEntities wellEntities)
            : base(logger, dapperProxy, userNameProvider)
        {
            this.wellEntities = wellEntities;
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

        public void Delete(int id)
        {
            var exceptionEvent = wellEntities.ExceptionEvent.Single(x => x.Id == id);
            wellEntities.ExceptionEvent.Remove(exceptionEvent);
            wellEntities.SaveChanges();
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

        public void InsertGrnEvent(GrnEvent grnEvent, DateTime dateCanBeProcessed,string jobId)
        {
            InsertEvent(grnEvent, EventAction.Grn, dateCanBeProcessed, jobId);
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
            InsertEvent(podTransaction,EventAction.PodTransaction, DateTime.Now);
        }

        public void InsertPodEvent(PodEvent podEvent, string jobId)
        {
            InsertEvent(podEvent, EventAction.Pod, DateTime.Now.Date.AddDays(1), jobId);
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
            InsertEvent(amendmentEvent, EventAction.Amendment, DateTime.Now);
        }

        public void InsertGlobalUpliftEvent(GlobalUpliftEvent glovalUpliftEvent, string sourceId = null)
        {
            InsertEvent(glovalUpliftEvent, EventAction.GlobalUplift, DateTime.Now, sourceId);
        }

        public bool GlobalUpliftEventCreatedForJob(string jobId)
        {
            return ExceptionEventCreatedForSourceId(jobId, EventAction.GlobalUplift);
        }

        private void InsertEvent(object eventData, EventAction action, DateTime dateCanBeProcessed,
            string sourceId = null)
        {
            string eventDataJson = null;
            if (eventData != null)
            {
                eventDataJson = JsonConvert.SerializeObject(eventData);
            }

            wellEntities.ExceptionEvent.Add(new Shared.Well.Data.EF.ExceptionEvent
            {
                Event = eventDataJson,
                ExceptionActionId = (int) action,
                DateCanBeProcessed = dateCanBeProcessed,
                SourceId = sourceId
            });

            wellEntities.SaveChanges();
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

        private ExceptionEvent GetExceptionEventBySourceId(string sourceId,EventAction eventAction)
        {
            ExceptionEvent result = null;
            var exceptionEvent = wellEntities.ExceptionEvent.FirstOrDefault(
                x => x.ExceptionActionId == (int)eventAction && x.SourceId == sourceId);

            if (exceptionEvent != null)
            {
                result = new ExceptionEvent
                {
                    Id = exceptionEvent.Id,
                    DateCanBeProcessed = exceptionEvent.DateCanBeProcessed,
                    ExceptionActionId = exceptionEvent.ExceptionActionId,
                    SourceId = exceptionEvent.SourceId,
                    UpdatedBy = exceptionEvent.UpdatedBy,
                    Event = exceptionEvent.Event,
                    Version = exceptionEvent.Version,
                    DateUpdated = exceptionEvent.DateUpdated,
                    CreatedBy = exceptionEvent.CreatedBy,
                    DateCreated = exceptionEvent.DateCreated,
                    Processed = exceptionEvent.Processed
                };
            }

            return result;
        }

        private bool ExceptionEventCreatedForSourceId(string sourceId, EventAction eventAction)
        {
            return wellEntities.ExceptionEvent.Any(
                x => x.ExceptionActionId == (int)eventAction && x.SourceId == sourceId);
        }
    }
}