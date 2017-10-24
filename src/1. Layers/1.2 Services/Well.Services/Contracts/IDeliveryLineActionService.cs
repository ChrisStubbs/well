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

        void Pod(PodEvent podEvent, int eventId, AdamSettings adamSettings);

        void PodTransaction(PodTransaction podTransaction, int eventId, AdamSettings adamSettings);

        void AmendmentTransaction(AmendmentTransaction amendmentTransaction, int eventId, AdamSettings adamSettings);
    }
}
