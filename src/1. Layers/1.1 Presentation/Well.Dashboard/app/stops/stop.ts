import * as _                               from 'lodash';
import { IFilter, GridHelpersFunctions }    from '../shared/gridHelpers/gridHelpers';

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
    public jobTypeAbbreviation: string;
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
    private resolution: string;
    public  resolutionId: number;
 
    public get barCode(): string
    {
        if (_.isNil(this.mBarCode))
        {
            this.mBarCode = this.noBarCode;
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

export class StopFilter implements IFilter
{
    constructor()
    {
        this.product = '';
        this.type = '';
        this.barCode = '';
        this.description = '';
        this.damages = undefined;
        this.shorts = undefined;
        this.checked = undefined;
        this.highValue = undefined;
        this.resolutionId = undefined;
    }

    public product: string;
    public type: string;
    public barCode: string;
    public description: string;
    public damages?: boolean;
    public shorts?: boolean;
    public checked: boolean;
    public highValue?: boolean;
    public resolutionId: string;

    public getFilterType(filterName: string): (value: any, value2: any) => boolean
    {
        switch (filterName)
        {
            case 'product':
            case 'description':
                return  GridHelpersFunctions.containsFilter;

            case 'type':
            case 'barCode':
                return  GridHelpersFunctions.isEqualFilter;

            case 'checked':
            case 'highValue':
                return  GridHelpersFunctions.boolFilter;

            case 'damages':
            case 'shorts':
                return (value: number, value2?: boolean) =>
                {
                    if (_.isNull(value2))
                    {
                        return true;
                    }
                    if (value2.toString() == 'true')
                    {
                        return value > 0;
                    }

                    return value == 0;
                };
            case 'resolutionId':
                return GridHelpersFunctions.resolutionFilter;
        }

        return undefined;
    }
}