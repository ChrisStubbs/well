import {DeliveryLineAction} from './deliveryLineAction';

export class SubmitLine {
    constructor(productCode: string, productDescription: string, actions: DeliveryLineAction[]) {
        this.productCode = productCode;
        this.productDescription = productDescription;
        this.actions = actions;
    }

    productCode: string;
    productDescription: string;
    actions: DeliveryLineAction[] = new Array<DeliveryLineAction>();
}