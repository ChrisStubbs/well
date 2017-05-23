import * as _ from 'lodash';
export class Stop 
{
    public routeId: number;
    public routeNumber: string;
    public branch: string;
    public branchId: number;
    public driver: string;
    public routeDate: Date;
    public assignedTo: string;
    public tba: number;
    public stopNo: string;
    public totalNoOfStopsOnRoute: number;
    public items: StopItem[];
}

export class StopItem
{
    constructor()
    {
        this.isSelected = false;
    }

    public noBarCode = 'NoBarCode';

    public jobId: number;
    public type: string;
    public invoice: string;
    public account: string;
    public accountID: number;
    public jobDetailId: number;
    public product: string;
    public description: string;
    public value: number;
    public invoiced: number;
    public delivered: number;
    public damages: number;
    public shorts: number;
    public checked: boolean;
    public highValue: boolean;
    private mBarCode: string;
    public isSelected: boolean;
    public lineItemId: number;
    public get barCode(): string
    {
        if (_.isNil(this.mBarCode))
        {
            this.mBarCode = '';
        }

        return this.mBarCode;
    }

    public set barCode(value: string)
    {
        this.mBarCode = value;
    }

    public get barCodeFilter(): string
    {
        if (this.barCode.length == 0)
        {
            return this.noBarCode;
        }

        return this.mBarCode;
    }

    public get tobacco(): string
    {
        return this.barCode.substr(this.barCode.length - 4, 4);
    }
}

export class StopFilter
{
    constructor()
    {
        this.type = '';
        this.tobacco = '';
        this.checked = '';
        this.heightValue = '';
    }

    public type: string;
    public tobacco: string;
    public checked: string;
    public heightValue: string;
}