import {Action} from './action';

export class DeliveryLineAction {

    constructor(index: number, quantity: number, action: Action) {
        this.index = index;
        this.quantity = quantity;
        this.action = action;
    }

    index: number;
    quantity: number;
    action: Action;
}