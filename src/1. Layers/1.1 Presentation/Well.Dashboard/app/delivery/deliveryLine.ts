﻿import {Damage} from './damage';

export class DeliveryLine {
    constructor(line: DeliveryLine) {
        if (line) {
            this.lineNo = line.lineNo;
            this.productCode = line.productCode;
            this.productDescription = line.productDescription;
            this.value = line.value;
            this.invoicedQuantity = line.invoicedQuantity;
            this.deliveredQuantity = line.deliveredQuantity;
            this.damagedQuantity = line.damagedQuantity;
            this.damagedQuantityOriginal = line.damagedQuantity;
            this.shortQuantity = line.shortQuantity;
            this.shortQuantityOriginal = line.shortQuantity;

            if (line.damages) {
                var index: number = 0;
                for (let damage of line.damages) {
                    this.damages.push(new Damage(index, damage.quantity, damage.reasonCode));
                    index++;
                }
            }
        }
    }

    lineNo: number;
    productCode: string;
    productDescription: string;
    value: string;
    invoicedQuantity: number;
    deliveredQuantity: number;
    damagedQuantity: number;
    damagedQuantityOriginal: number;
    shortQuantity: number;
    shortQuantityOriginal: number;
    isEdit: boolean = false;
    damages: Damage[] = new Array<Damage>();
}