export class Damage {

    constructor(index: number, quantity: number, reasonCode: string) {
        this.index = index;
        this.quantity = quantity;
        this.reasonCode = reasonCode;
    }

    index: number;
    quantity: number;
    reasonCode: string;
}