import {DeliveryLine} from './deliveryLine';

export class Delivery {
    constructor() {
        this.deliveryLines = [];
    }
    id: number;
    accountCode: string;
    accountName: string;
    accountAddress: string;
    invoiceNumber: string;
    contactName: string;
    phoneNumber: string;
    mobileNumber: string;
    deliveryLines: DeliveryLine[];
}