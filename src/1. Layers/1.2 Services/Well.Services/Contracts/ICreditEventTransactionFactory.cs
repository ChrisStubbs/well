namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using Domain.ValueObjects;

    public interface ICreditEventTransactionFactory
    {
        CreditEventTransaction BuildCreditEventTransaction(IList<DeliveryLine> deliveryLines, string username);
    }
}
