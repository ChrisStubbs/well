using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PH.Well.Domain.Enums;
using PH.Well.Domain.ValueObjects;

namespace PH.Well.Services.Contracts
{
    public interface IDeliveryLinesAction
    {
        ProcessDeliveryActionResult Execute(Func<DeliveryAction, IList<DeliveryLine>> deliveryLines, AdamSettings adamSettings, string username, int branchId);
        DeliveryAction Action { get; }
    }
}
