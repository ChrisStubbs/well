namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    public interface IExceptionEventService
    {
        void Credit(CreditEvent creditEvent, AdamSettings adamSettings);

        AdamResponse Credit(CreditEvent creditEvent, AdamSettings adamSettings, string username);

        void CreditReorder(CreditReorderEvent creditReorderEvent, AdamSettings adamSettings);

        AdamResponse CreditReorder(CreditReorderEvent creditReorderEvent, AdamSettings adamSettings, string username);

        void Reject(RejectEvent rejectEvent, AdamSettings adamSettings);

        AdamResponse Reject(RejectEvent rejectEvent, AdamSettings adamSettings, string username);

        void ReplanRoadnet(RoadnetEvent roadnetEvent, AdamSettings adamSettings);

        AdamResponse ReplanRoadnet(RoadnetEvent roadnetEvent, AdamSettings adamSettings, string username);

        void ReplanTranscend(TranscendEvent transcendEvet, AdamSettings adamSettings);

        AdamResponse ReplanTranscend(TranscendEvent transcendEvet, AdamSettings adamSettings, string username);

        void ReplanQueue(QueueEvent queueEvent, AdamSettings adamSettings);

        AdamResponse ReplanQueue(QueueEvent queueEvent, AdamSettings adamSettings, string username);
    }
}