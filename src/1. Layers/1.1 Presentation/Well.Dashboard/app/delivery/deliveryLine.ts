﻿export class DeliveryLine {
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
            this.reason = line.reason;
            this.reasonOriginal = line.reason;
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
    reason: string;
    reasonOriginal: string;
    isEdit: boolean = false;
}