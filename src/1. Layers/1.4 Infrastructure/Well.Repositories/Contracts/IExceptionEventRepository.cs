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

        void MarkEventAsProcessed(int eventId);

        IEnumerable<ExceptionEvent> GetAllUnprocessed();

        void RemovedPendingCredit(int jobId);

        void InsertGrnEvent(GrnEvent grnEvent, DateTime dateCanBeProcessed);

        void InsertPodEvent(PodEvent podEvent);

        void InsertPodTransaction(PodTransaction podTransaction);

        void InsertAmendmentTransaction(AmendmentTransaction amendmentTransaction);

        Task InsertAmendmentTransactionAsync(IList<AmendmentTransaction> amendmentEvent);

        void InsertEvent(EventAction action, object eventData, DateTime? dateCanBeProcessed = null);
    }
}