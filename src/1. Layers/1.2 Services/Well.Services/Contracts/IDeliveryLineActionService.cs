namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.ValueObjects;

    public interface IDeliveryLineActionService
    {
        void CreditTransaction(CreditTransaction creditTransaction, int eventId, AdamSettings adamSettings, string username);

        void Grn(GrnEvent grnEvent, int eventId, AdamSettings adamSettings, string username);

        //AdamResponse Grn(GrnEvent grnEvent, AdamSettings adamSettings, string username);

        Task<ProcessDeliveryActionResult> ProcessDeliveryActions(List<DeliveryLine> lines, AdamSettings adamSettings, string username, int branchId);
    }
}