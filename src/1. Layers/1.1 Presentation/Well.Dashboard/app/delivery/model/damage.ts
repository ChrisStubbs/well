export class Damage {

    constructor(index: number, quantity: number, jobDetailReasonId: number, jobDetailSourceId: number) {
        this.index = index;
        this.quantity = quantity;
        this.jobDetailReasonId = jobDetailReasonId;
        this.jobDetailSourceId = jobDetailSourceId;
    }

    public index: number;
    public quantity: number;
    public jobDetailReasonId: number;
    public jobDetailSourceId: number;
}