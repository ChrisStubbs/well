﻿import {Damage} from './damage';
import {DeliveryLineAction} from './deliveryLineAction';

export class DeliveryLine {
    constructor(line: DeliveryLine) {
        if (line) {
            this.jobDetailId = line.jobDetailId;
            this.jobId = line.jobId;
            this.lineNo = line.lineNo;
            this.productCode = line.productCode;
            this.productDescription = line.productDescription;
            this.value = line.value;
            this.invoicedQuantity = line.invoicedQuantity;
            this.deliveredQuantity = line.deliveredQuantity;
            this.damagedQuantity = line.damagedQuantity;
            this.shortQuantity = line.shortQuantity;
            this.lineDeliveryStatus = line.lineDeliveryStatus;

            if (line.damages) {
                var index: number = 0;
                for (let damage of line.damages) {
                    this.damages.push(new Damage(index, damage.quantity, damage.reasonCode));
                    index++;
                }
            }

            if (line.actions) {
                var index: number = 0;
                for (let a of line.actions) {
                    this.actions.push(new DeliveryLineAction(index, a.quantity, a.action, a.actionDescription, a.status, a.statusDescription));
                    index++;
                }
            }

            this.isCleanOnInit = this.isClean();
        }
    }

    jobDetailId: number;
    jobId: number;
    lineNo: number;
    productCode: string;
    productDescription: string;
    value: string;
    invoicedQuantity: number;
    deliveredQuantity: number;
    damagedQuantity: number;
    shortQuantity: number;
    lineDeliveryStatus: string;
    damages: Damage[] = new Array<Damage>();
    actions: DeliveryLineAction[] = new Array<DeliveryLineAction>();

    isCleanOnInit: boolean;

    isClean(): boolean {
        if (this.shortQuantity > 0) {
            return false;
        }

        for (let damage of this.damages) {
            if (damage.quantity > 0) {
                return false;
            }
        }

        return true;
    }

    isDetailChecked(): boolean {

        if (this.lineDeliveryStatus === "Exception") {
            return true;
        }
        if (this.lineDeliveryStatus === "Delivered") {
            return true;
        }
        if (this.lineDeliveryStatus === "Unknown") {
            return false;
        }

        return false;
    }
}