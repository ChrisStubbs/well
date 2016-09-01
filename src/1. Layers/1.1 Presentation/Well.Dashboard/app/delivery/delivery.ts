import {DeliveryLine} from './deliveryLine';

export class Delivery {
    constructor(delivery: Delivery) {
        if (delivery) {
            this.id = delivery.id;
            this.accountCode = delivery.accountCode;
            this.accountName = delivery.accountName;
            this.accountAddress = delivery.accountAddress;
            this.invoiceNumber = delivery.invoiceNumber;
            this.contactName = delivery.contactName;
            this.phoneNumber = delivery.phoneNumber;
            this.mobileNumber = delivery.mobileNumber;
            this.deliveryType = delivery.deliveryType;
            this.isException = delivery.isException;

            if (delivery.deliveryLines) {
                for (let line of delivery.deliveryLines) {
                    this.deliveryLines.push(new DeliveryLine(line));
                }
            }
        }
    }
    id: number;
    accountCode: string;
    accountName: string;
    accountAddress: string;
    invoiceNumber: string;
    contactName: string;
    phoneNumber: string;
    mobileNumber: string;
    deliveryType: string;
    isException: boolean;
    deliveryLines: DeliveryLine[] = new Array<DeliveryLine>();

    isCleanOnInit(): boolean {
        var clean = true;
        for (let line of this.deliveryLines) {
            if (line.isCleanOnInit === false) {
                clean = false;
            }
        }
        return clean;
    };

    isClean(): boolean {
        var clean = true;
        for (let line of this.deliveryLines) {
            if (line.isClean() === false) {
                clean = false;
            }
        }
        return clean;
    }
}