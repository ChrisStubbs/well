import {IFilter}                from '../shared/gridHelpers/IFilter';
import {GridHelpersFunctions}   from '../shared/gridHelpers/gridHelpersFunctions';
import * as _                   from 'lodash';

export class ActivitySource
{
    constructor()
    {
        this.details = [];
    }

    public branch: string;
    public branchId: number;
    public primaryAccount: string;
    public accountName: string;
    public accountNumber: string;
    public accountAddress: string;
    public pod: boolean;
    public cod: boolean;
    public itemNumber: string;
    public isInvoice: boolean;
    public date: Date;
    public driver: string;
    public assignee: string;
    public tba: number;
    public resolution: string;
    public resolutionId: number;
    public locationId: number;
    public details: Array<ActivitySourceDetail>;
    public initialDocument: string;
}

export class ActivitySourceGroup
{
    constructor()
    {
        this.isExpanded = false;
    }

    public isExpanded: boolean;
    public stopId: number;
    public stop: string;
    public stopDate: Date;
    public jobId: number;
    public type: string;
    public totalDamaged: number;
    public totalShorts: number;
    public totalExpected: number;
    public totalActual: number;
    public details: Array<ActivitySourceDetail>;
    public resolution: string;
    public resolutionId: number;
    public completedOnPaper: boolean;

}

export class ActivitySourceDetail
{
    public noBarCode = 'NoBarCode';

    constructor()
    {
        this.isSelected = false;
    }

    public product: string;
    public type: string;
    public description: string;
    public value: number;
    public actual: number;
    public expected: number;
    public damaged: number;
    public shorts: number;
    public checked: boolean;
    public highValue: boolean;
    public resolution: string;
    public resolutionId: number;
    public stopId: number;
    public stop: string;
    public stopDate: Date;
    public jobId: number;
    public jobType: string;
    public jobTypeAbbreviation: string;
    public lineItemId: number;
    public hasUnresolvedActions: boolean;
    public hasLineItemActions: boolean;
    public completedOnPaper: boolean;
    public get exceptionsFilter(): number
    {
        let result: number = 0;

        result = result
            | (this.damaged / this.damaged)
            | ((this.shorts / this.shorts) * 2);

        return result || 4;
    }

    private mBarCode: string;
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

    public isSelected: boolean;
    public upliftAction: number;
}

export class ActivityFilter implements IFilter
{
    constructor()
    {
        this.product = '';
        this.type = '';
        this.barCode = '';
        this.description = '';
        this.exceptions = '';
        this.checked = undefined;
        this.highValue = undefined;
        this.resolutionId = undefined;
        this.exceptionsFilter = 0;
    }

    public product: string;
    public type: string;
    public barCode: string;
    public description: string;
    public exceptions: string;
    public checked: boolean;
    public highValue?: boolean;
    public resolutionId: number;
    public exceptionsFilter: number;

    public getFilterType(filterName: string): (value: any, value2: any, sourceRow: ActivitySourceDetail) => boolean
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
                return (value: number, value2: number, sourceRow: ActivitySourceDetail) => {
                    let actionRequiredFilter: boolean = true;

                    if (value2 == 4) {
                        actionRequiredFilter = sourceRow.hasUnresolvedActions;
                    }

                    return  actionRequiredFilter && GridHelpersFunctions.enumBitwiseAndCompare(value, value2);
                };

            case 'exceptionsFilter':
                return (value: number, value2: number, sourceRow: ActivitySourceDetail) =>
                {
                    return  GridHelpersFunctions.enumBitwiseAndCompare(value, value2) ||
                            GridHelpersFunctions.enumBitwiseAndCompare(value2, value);
                };
        }

        return undefined;
    }
}
