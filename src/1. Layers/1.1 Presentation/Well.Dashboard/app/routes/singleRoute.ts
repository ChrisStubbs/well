import * as _                   from 'lodash';
import {IFilter}                from '../shared/gridHelpers/IFilter';
import { GridHelpersFunctions } from '../shared/gridHelpers/gridHelpersFunctions';
import { LookupService } from '../shared/services/lookupService';
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
    public stopExceptions: number;
    public stopClean: number;
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
    public wellStatus: number;
    public wellStatusDescription: string;
    public grnNumber: string;
    public grnProcessType: number;
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
}

export class SingleRouteFilter implements IFilter
{
    constructor()
    {
        this.account = '';
        this.invoice = '';
        this.jobType = '';
        this.wellStatus = '';
        this.exceptions = undefined;
        this.clean = undefined;
        this.assignee = '';
        this.resolutionId = undefined;
    }

    public account: string;
    public invoice: string;
    public jobType: string = '';
    public wellStatus: string;
    public exceptions: boolean;
    public clean: boolean;
    public assignee: string;
    public resolutionId: number;

    public getFilterType(filterName: string): (value: any, value2: any) => boolean
    {
        switch (filterName)
        {
            case 'account':
            case 'invoice':
                return  GridHelpersFunctions.containsFilter;

            case 'jobType':
                return GridHelpersFunctions.startsWithFilter;

            case 'wellStatus':
                return GridHelpersFunctions.isEqualFilter;

            case 'assignee':
                return  GridHelpersFunctions.isEqualFilter;

            case 'exceptions':
            case 'clean':
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
                return LookupService.compareResolutionStatusValue;
        }

        return undefined;
    }
}