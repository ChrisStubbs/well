import * as _                               from 'lodash';
import { IFilter, GridHelpersFunctions }    from '../shared/gridHelpers/gridHelpers';
import { IGrnAssignable }                   from '../job/job';

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

export class StopItem implements IGrnAssignable
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
    public bypassed: number;
    public checked: boolean;
    public highValue: boolean;
    private mBarCode: string;
    public isSelected: boolean;
    public lineItemId: number;
    private resolution: string;
    public resolutionId: number;
    public grnProcessType: number;
    public hasUnresolvedActions: boolean;
    public grnNumber: string;
 
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

    public get exceptionsFilter(): number
    {
        let result: number = 0;

        result = result
            | (this.damages / this.damages)
            | ((this.shorts / this.shorts) * 2);

        return result || 4;
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
        this.checked = undefined;
        this.highValue = undefined;
        this.resolutionId = undefined;
        this.exceptionsFilter = 0;
    }

    public product: string;
    public type: string;
    public barCode: string;
    public description: string;
    public checked: boolean;
    public highValue?: boolean;
    public resolutionId: number;
    public exceptionsFilter: number;

    public getFilterType(filterName: string): (value: any, value2: any) => boolean
    {
        switch (filterName)
        {
            case 'product':
            case 'description':
                return  GridHelpersFunctions.containsFilter;

            case 'type':
                return GridHelpersFunctions.startsWithFilter;

            case 'barCode':
                return  GridHelpersFunctions.isEqualFilter;

            case 'checked':
            case 'highValue':
                return  GridHelpersFunctions.boolFilter;

            case 'resolutionId':
                return GridHelpersFunctions.enumBitwiseAndCompare;

            case 'exceptionsFilter':
                return (value: number, value2: number) =>
                {
                    return  GridHelpersFunctions.enumBitwiseAndCompare(value, value2) ||
                        GridHelpersFunctions.enumBitwiseAndCompare(value2, value);
                }
        }

        return undefined;
    }
}