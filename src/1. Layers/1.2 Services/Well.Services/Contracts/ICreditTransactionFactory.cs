namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using Domain.ValueObjects;

    public interface ICreditTransactionFactory
    {
        CreditTransaction Build(List<DeliveryLineCredit> deliveryLines, string username, int branchId);
    }
}
