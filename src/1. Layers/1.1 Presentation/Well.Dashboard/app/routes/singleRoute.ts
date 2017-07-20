import * as _                   from 'lodash';
import {IFilter}                from '../shared/gridHelpers/IFilter';
import { GridHelpersFunctions } from '../shared/gridHelpers/gridHelpersFunctions';
import {IGrnAssignable} from '../job/job';

export interface SingleRoute
{
    id: number;
    routeNumber: string;
    branch: string;
    branchId: number;
    driver: string;
    routeDate?: Date;
    items: SingleRouteItem[];
}

export class SingleRouteItem implements IGrnAssignable
{
    constructor()
    {
        this.isSelected = false;
    }

    public jobId: number;
    public stopId: number;
    public stop: string;
    public stopStatus: string;
    public tba: number;
    public stopAssignee: string;
    public resolution: string;
    public resolutionId: number;
    public invoice: string;
    public jobType: string;
    public jobTypeId: number;
    public cod: string;
    public get isCod(): boolean
    {
        return _.isNil(this.cod) || this.cod != '';
    }
    public pod: boolean;
    public exceptions: number;
    public invoicedQty: number;
    public clean: number;
    public credit?: number;
    public assignee: string;
    public selected?: boolean;
    public jobStatusDescription: string;
    public jobStatus: number;
    public isSelected: boolean;
    public account: string;
    public accountName: string;
    public wellStatus: number;
    public wellStatusDescription: string;
    public grnNumber: string;
    public grnProcessType: number;
    public primaryAccountNumber: string;
}

export class SingleRouteSource
{
    constructor()
    {
        this.isExpanded = false;
    }

    public stopId: number;
    public stop: string;
    public stopStatus: string;
    public totalExceptions: number;
    public totalClean: number;
    public totalTBA: number;
    public stopAssignee: string;
    public isExpanded: boolean;
    public items: Array<SingleRouteItem>;
    public accountName: string;
}

export class SingleRouteFilter implements IFilter
{
    constructor()
    {
        this.account = '';
        this.invoice = '';
        this.jobTypeId = undefined;
        this.wellStatus = '';
        this.exceptions = undefined;
        this.assignee = '';
        this.resolutionId = undefined;
    }

    public account: string;
    public invoice: string;
    public jobTypeId?: number = undefined;
    public wellStatus: string;
    public exceptions: boolean;
    public assignee: string;
    public resolutionId: number;

    public getFilterType(filterName: string): (value: any, value2: any, sourceRow: any) => boolean
    {
        switch (filterName)
        {
            case 'account':
            case 'invoice':
                return  GridHelpersFunctions.containsFilter;

            case 'jobTypeId':
                 return (value: number, value2: number) =>
                    {
                        const v = +value;
                        const v2 = +value2;

                        const f = GridHelpersFunctions.isEqualFilter;

                        return  f(v, v2);
                    };

            case 'wellStatus':
            case 'assignee':
                return  GridHelpersFunctions.isEqualFilter;

            case 'exceptions':
                return (value: number, value2: number, sourceRow: SingleRouteItem) => {

                    if (+value2 == 1)
                    {
                        return sourceRow.exceptions > 0;
                    }

                    return sourceRow.clean > 0;
                };

            case 'resolutionId':
                return GridHelpersFunctions.enumBitwiseAndCompare;
        }

        return undefined;
    }
}