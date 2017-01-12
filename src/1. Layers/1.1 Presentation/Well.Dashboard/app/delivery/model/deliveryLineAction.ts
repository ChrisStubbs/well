import {Action} from './action';

export class DeliveryLineAction {

    constructor(
        index: number,  quantity: number,
        action: number, actionDescription: string,
        status: number, statusDescription: string) {

        this.index = index;
        this.quantity = quantity;
        this.action = action;
        this.actionDescription = actionDescription;
        this.status = status;
        this.statusDescription = statusDescription;
    }

    public index: number;
    public quantity: number;
    public action: number;
    public actionDescription: string;
    public status: number;
    public statusDescription: string;
}