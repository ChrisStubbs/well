﻿import { Damage } from './damage';
import * as lodash from 'lodash';

export class DeliveryLine
{
    constructor(line: DeliveryLine)
    {
        if (line)
        {
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
            this.jobDetailReasonId = line.jobDetailReasonId;
            this.jobDetailSourceId = line.jobDetailSourceId;
            this.shortsActionId = line.shortsActionId;
            this.isHighValue = line.isHighValue;

            if (line.damages)
            {
                let index = 0;
                for (const damage of line.damages)
                {
                    this.damages.push(new Damage(
                        index,
                        damage.quantity,
                        damage.jobDetailReasonId,
                        damage.jobDetailSourceId,
                        damage.damageActionId));
                    index++;
                }
            }

            this.isCleanOnInit = this.isClean();
        }
    }

    public jobDetailId: number;
    public jobId: number;
    public lineNo: number;
    public productCode: string;
    public productDescription: string;
    public value: string;
    public invoicedQuantity: number;
    public deliveredQuantity: number;
    public damagedQuantity: number;
    public shortQuantity: number;
    public lineDeliveryStatus: string;
    public jobDetailReasonId: number;
    public jobDetailSourceId: number;
    public shortsActionId: number;
    public isHighValue: boolean;
    public jobDetailReason: string;
    public jobDetailSource: string;
    public shortsAction: string;
    public totalCreditThreshold: string;
    public damages: Damage[] = new Array<Damage>();
    public isCleanOnInit: boolean;

    public isClean(): boolean
    {
        if (this.shortQuantity > 0)
        {
            return false;
        }

        for (const damage of this.damages)
        {
            if (damage.quantity > 0)
            {
                return false;
            }
        }

        return true;
    }   

    public isDetailChecked(): boolean
    {
        if (this.lineDeliveryStatus === 'Exception') {
            return true;
        }
        if (this.lineDeliveryStatus === 'Delivered') {
           return true;
        }
        if (this.lineDeliveryStatus === 'Unknown') {
           return false;
        }
          
        return false;
    } 

    public totalQtyOfShortsAndDamages(): number
    {
        return this.shortQuantity + lodash.sum(lodash.map(this.damages, 'quantity'));
    }

    public canSave(): boolean
    {
        return this.totalQtyOfShortsAndDamages() <= this.invoicedQuantity;
    }
}