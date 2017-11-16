using System;
using PH.Well.Domain.Enums;

namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;

    public interface IExceptionEventRepository : IRepository<ExceptionEvent, int>
    {
        void InsertCreditEventTransaction(CreditTransaction creditTransaction);

        void InsertUpliftEventTransaction(CreditTransaction upliftTransaction);

        void MarkEventAsProcessed(int eventId);

        void Delete(int id);

        IEnumerable<ExceptionEvent> GetAllUnprocessed();

        void RemovedPendingCredit(int jobId);

        void InsertGrnEvent(GrnEvent grnEvent, DateTime dateCanBeProcessed, string jobId);

        bool IsGrnEventCreatedForJob(string jobId);

        void InsertPodEvent(PodEvent podEvent, string jobId,  DateTime dateCanBeProcessed);

        bool IsPodEventCreatedForJob(string jobId);

        void InsertPodTransaction(PodTransaction podTransaction);

        void InsertAmendmentTransaction(AmendmentTransaction amendmentTransaction);

        void InsertGlobalUpliftEvent(GlobalUpliftEvent glovalUpliftEvent, string jobId = null);

        bool IsGlobalUpliftEventCreatedForJob(string jobId);

        bool IsPodTransactionCreatedForJob(string jobId);

    }
}