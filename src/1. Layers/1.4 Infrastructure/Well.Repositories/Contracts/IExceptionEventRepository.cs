namespace PH.Well.Repositories.Contracts
{
    using PH.Well.Domain.ValueObjects;

    public interface IExceptionEventRepository
    {
        void InsertCreditEvent(CreditEvent creditEvent);
    }
}