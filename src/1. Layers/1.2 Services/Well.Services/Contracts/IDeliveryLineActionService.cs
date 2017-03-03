namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;
    using Domain.ValueObjects;

    public interface IDeliveryLineActionService
    {
        void CreditTransaction(CreditTransaction creditTransaction, int eventId, AdamSettings adamSettings);

        void Grn(GrnEvent grnEvent, int eventId, AdamSettings adamSettings);

        //AdamResponse Grn(GrnEvent grnEvent, AdamSettings adamSettings, string username);

        ProcessDeliveryActionResult ProcessDeliveryActions(Job job);

        void Pod(PodTransaction podTransaction, int eventId, AdamSettings adamSettings);

        //ProcessDeliveryPodActionResult ProcessDeliveryPodActions(List<DeliveryLine> lines, AdamSettings adamSettings,string username, int branchId);
    }
}
