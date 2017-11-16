namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using Domain.ValueObjects;

    public interface IUpliftTransactionFactory
    {
        CreditTransaction Build(List<DeliveryLineUplift> deliveryLines, int branchId);
    }
}
