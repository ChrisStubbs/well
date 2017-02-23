export class Damage
{

    constructor(
        index: number,
        quantity: number,
        jobDetailReasonId: number,
        jobDetailSourceId: number,
        damageActionId: number)
    {
        this.index = index;
        this.quantity = quantity;
        this.jobDetailReasonId = jobDetailReasonId;
        this.jobDetailSourceId = jobDetailSourceId;
        this.damageActionId = damageActionId;
    }

    public index: number;
    public quantity: number;
    public jobDetailReasonId: number;
    public jobDetailSourceId: number;
    public damageActionId: number;
    public jobDetailReason: string;
    public jobDetailSource: string;
    public damageAction: string;
}