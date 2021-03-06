﻿namespace PH.Well.Repositories.Contracts
{
    using Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    public interface IAdamRepository
    {
        AdamResponse Credit(CreditTransaction creditTransaction, AdamSettings adamSettings);

        //    AdamResponse CreditHeader(CreditEvent credit, AdamSettings adamSettings);

        //    AdamResponse CreditLines(CreditEvent credit, AdamSettings adamSettings);

        AdamResponse CreditReorder(CreditReorderEvent creditReorder, AdamSettings adamSettings);

        AdamResponse Reject(RejectEvent reject, AdamSettings adamSettings);

        AdamResponse ReplanRoadnet(RoadnetEvent roadnet, AdamSettings adamSettings);

        AdamResponse ReplanTranscend(TranscendEvent transcend, AdamSettings adamSettings);

        AdamResponse ReplanQueue(QueueEvent queue, AdamSettings adamSettings);

        AdamResponse Grn (GrnEvent grn, AdamSettings adamSettings);

        AdamResponse Pod (PodEvent podEvent, AdamSettings adamSettings, Job job);

        AdamResponse PodTransaction(PodTransaction podTransaction, AdamSettings adamSettings);

        AdamResponse AmendmentTransaction(AmendmentTransaction amend, AdamSettings adamSettings);
        AdamResponse GlobalUplift(GlobalUpliftTransaction globalUpliftTransaction, AdamSettings adamSettings);

    }
}