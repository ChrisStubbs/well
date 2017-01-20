namespace PH.Well.Services.Contracts
{
    using Domain.ValueObjects;

    public interface ICreditEventTransactionFactory
    {
        CreditEventTransaction BuildCreditEventTransaction(CreditEvent credit, string username);
    }
}
