import * as _ from 'lodash';

export class Stop
{
    public noBarCode = 'NoBarCode';

    public id: number;
    public job: string;
    public invoice: string;
    public account: string;
    public product: string;
    public type: string;
    public description: string;
    public value: number;
    public invoiced: number;
    public delivered: number;
    public damages: number;
    public shorts: number;
    public checked: boolean;
    public heightValue: boolean;

    private mBarCode: string;
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

    public get tobacco (): string
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