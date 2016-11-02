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
            this.cashOnDelivery = delivery.cashOnDelivery;
            this.isException = delivery.isException;
            this.canAction = delivery.canAction;
            this.canSubmit = delivery.canSubmit;

            if (delivery.exceptionDeliveryLines) {
                for (let line of delivery.exceptionDeliveryLines) {
                    this.exceptionDeliveryLines.push(new DeliveryLine(line));
                }
            }

            if (delivery.cleanDeliveryLines) {
                for (let line of delivery.cleanDeliveryLines) {
                    this.cleanDeliveryLines.push(new DeliveryLine(line));
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
    cashOnDelivery: string;
    isException: boolean;
    canAction: boolean;
    canSubmit: boolean;
    exceptionDeliveryLines: DeliveryLine[] = new Array<DeliveryLine>();
    cleanDeliveryLines: DeliveryLine[] = new Array<DeliveryLine>();

    isCleanOnInit(): boolean {
        var clean = true;

        for (let line of this.exceptionDeliveryLines) {
            if (line.isCleanOnInit === false) {
                clean = false;
            }
        }

        for (let line of this.cleanDeliveryLines) {
            if (line.isCleanOnInit === false) {
                clean = false;
            }
        }

        return clean;
    };

    isClean(): boolean {
        var clean = true;

        for (let line of this.exceptionDeliveryLines) {
            if (line.isClean() === false) {
                clean = false;
            }
        }

        for (let line of this.cleanDeliveryLines) {
            if (line.isClean() === false) {
                clean = false;
            }
        }

        return clean;
    }
}