import {Action} from './action';

export class DeliveryLineAction {

    constructor(index: number, quantity: number, action: number, status: number, statusDescription: string) {
        this.index = index;
        this.quantity = quantity;
        this.action = action;
        if (status) {
            this.status = status;
        }
        if (statusDescription) {
            this.statusDescription = statusDescription;
        }
    }

    index: number;
    quantity: number;
    action: number;
    status: number;
    statusDescription: string;
}