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

        void InsertGrnEvent(GrnEvent grnEvent, DateTime dateCanBeProcessed, string jobId);

        ExceptionEvent GetGrnExceptionEvent(string jobId);

        void InsertPodEvent(PodEvent podEvent, string jobId);

        ExceptionEvent GetPodExceptionEvent(string jobId);

        void InsertPodTransaction(PodTransaction podTransaction);

        void InsertAmendmentTransaction(AmendmentTransaction amendmentTransaction);

        void InsertGlobalUpliftEvent(GlobalUpliftEvent glovalUpliftEvent);

        Task InsertAmendmentTransactionAsync(IList<AmendmentTransaction> amendmentEvent);
    }
}