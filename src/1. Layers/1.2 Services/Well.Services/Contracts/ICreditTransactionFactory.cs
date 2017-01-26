namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using Domain.ValueObjects;

    public interface ICreditTransactionFactory
    {
        CreditTransaction BuildCreditEventTransaction(IList<DeliveryLine> deliveryLines, string username);
    }
}
