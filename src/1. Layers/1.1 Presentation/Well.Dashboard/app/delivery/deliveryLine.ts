import {Damage} from './damage';

export class DeliveryLine {
    constructor(line: DeliveryLine) {
        if (line) {
            this.jobId = line.jobId;
            this.lineNo = line.lineNo;
            this.productCode = line.productCode;
            this.productDescription = line.productDescription;
            this.value = line.value;
            this.invoicedQuantity = line.invoicedQuantity;
            this.deliveredQuantity = line.deliveredQuantity;
            this.damagedQuantity = line.damagedQuantity;
            this.shortQuantity = line.shortQuantity;

            if (line.damages) {
                var index: number = 0;
                for (let damage of line.damages) {
                    this.damages.push(new Damage(index, damage.quantity, damage.reasonCode));
                    index++;
                }
            }

            this.isCleanOnInit = this.isClean();
        }
    }

    jobId: number;
    lineNo: number;
    productCode: string;
    productDescription: string;
    value: string;
    invoicedQuantity: number;
    deliveredQuantity: number;
    damagedQuantity: number;
    shortQuantity: number;
    damages: Damage[] = new Array<Damage>();

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
}