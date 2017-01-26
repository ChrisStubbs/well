namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    public interface IExceptionEventService
    {
        void CreditEventTransaction(CreditEventTransaction creditEventTransaction, int eventId, AdamSettings adamSettings, string username);

        void Grn(GrnEvent grnEvent, int eventId, AdamSettings adamSettings, string username);

        AdamResponse Grn(GrnEvent grnEvent, AdamSettings adamSettings, string username);

        ProcessDeliveryActionResult ProcessDeliveryActions(IList<DeliveryLine> lines, AdamSettings adamSettings, string username, int branchId);
    }
}