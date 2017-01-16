namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    public interface IExceptionEventService
    {
        void Credit(CreditEvent creditEvent, int eventId, AdamSettings adamSettings, string username);

        AdamResponse BulkCredit(IEnumerable<CreditEvent> creditEvents, string username);

        AdamResponse Credit(CreditEvent creditEvent, AdamSettings adamSettings, string username);

        void CreditReorder(CreditReorderEvent creditReorderEvent, int eventId, AdamSettings adamSettings, string username);

        AdamResponse CreditReorder(CreditReorderEvent creditReorderEvent, AdamSettings adamSettings, string username);

        void Reject(RejectEvent rejectEvent, int eventId, AdamSettings adamSettings, string username);

        AdamResponse Reject(RejectEvent rejectEvent, AdamSettings adamSettings, string username);

        void ReplanRoadnet(RoadnetEvent roadnetEvent, int eventId, AdamSettings adamSettings, string username);

        AdamResponse ReplanRoadnet(RoadnetEvent roadnetEvent, AdamSettings adamSettings, string username);

        void ReplanTranscend(TranscendEvent transcendEvet, int eventId, AdamSettings adamSettings, string username);

        AdamResponse ReplanTranscend(TranscendEvent transcendEvet, AdamSettings adamSettings, string username);

        void ReplanQueue(QueueEvent queueEvent, int eventId, AdamSettings adamSettings, string username);

        AdamResponse ReplanQueue(QueueEvent queueEvent, AdamSettings adamSettings, string username);

        void Grn(GrnEvent grnEvent, int eventId, AdamSettings adamSettings, string username);

        AdamResponse Grn(GrnEvent grnEvent, AdamSettings adamSettings, string username);
    }
}