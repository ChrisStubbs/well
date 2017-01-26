namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using Domain.ValueObjects;

    public interface ICreditTransactionFactory
    {
        CreditTransaction Build(IList<DeliveryLine> deliveryLines, string username);
    }
}
