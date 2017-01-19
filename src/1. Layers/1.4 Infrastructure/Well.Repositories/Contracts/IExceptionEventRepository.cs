namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;

    public interface IExceptionEventRepository : IRepository<ExceptionEvent, int>
    {
        void InsertCreditEvent(CreditEvent creditEvent);

        void MarkEventAsProcessed(int eventId);

        IEnumerable<ExceptionEvent> GetAllUnprocessed();

        void RemovedPendingCredit(string invoiceNumber);

        void InsertGrnEvent(GrnEvent grnEvent);
    }
}