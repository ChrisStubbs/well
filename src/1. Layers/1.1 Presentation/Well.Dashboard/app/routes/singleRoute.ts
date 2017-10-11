import { Assignee }             from './../shared/models/assignee';
import * as _                   from 'lodash';
import { IFilter }              from '../shared/gridHelpers/IFilter';
import { GridHelpersFunctions } from '../shared/gridHelpers/gridHelpersFunctions';
import { IGrnAssignable }       from '../job/job';
import { GrnHelpers }           from '../job/assignGrnModal';

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
    public previously: string;
    public stopStatus: string;
    public tba: number;
    public stopAssignee: string;
    public assignees: Assignee[];
    public resolution: string;
    public resolutionId: number;
    public invoice: string;
    public invoiceId: number;
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
    public locationId: number;
    public hasUnresolvedActions: boolean;
    public completedOnPaper: boolean;
    public get uncompletedJob(): boolean
    {
        return this.hasUnresolvedActions
            || (GrnHelpers.isGrnRequired(this) && _.isEmpty(this.grnNumber));
    }
}

export class SingleRouteSource
{
    constructor()
    {
        this.isExpanded = false;
    }

    public stopId: number;
    public stop: string;
    public previously: string;
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
        this.uncompletedJob = undefined;
    }

    public account: string;
    public invoice: string;
    public jobTypeId?: number = undefined;
    public wellStatus: string;
    public exceptions: boolean;
    public assignee: string;
    public resolutionId: number;
    public uncompletedJob: boolean;

    public getFilterType(filterName: string): (value: any, value2: any, sourceRow: any) => boolean
    {
        switch (filterName)
        {
            case 'account':
            case 'invoice':
                return  GridHelpersFunctions.containsFilter;

            case 'uncompletedJob':
                return  GridHelpersFunctions.boolFilter;

            case 'jobTypeId':
                 return (value: number, value2: number) =>
                    {
                        const v = +value;
                        const v2 = +value2;

                        const f = GridHelpersFunctions.isEqualFilter;

                        return  f(v, v2);
                    };

            case 'wellStatus':
                return (value: any, valu2: any) => {
                    return GridHelpersFunctions.isEqualFilter(String(value), String(valu2));
                };

            case 'assignee':
                return (value: string, value2: string, sourceRow: SingleRouteItem) =>
                {
                    if (_.isNil(sourceRow.assignees) || sourceRow.assignees.length == 0)
                    {
                        return value2 == 'Unallocated';
                    }

                    return sourceRow.assignees.some((current: Assignee) => current.name == value2);
                };

            case 'exceptions':
                return (value: number, value2: number, sourceRow: SingleRouteItem) => {

                    if (+value2 == 1)
                    {
                        return sourceRow.exceptions > 0;
                    }

                    return sourceRow.clean > 0 && sourceRow.exceptions == 0;
                };

            case 'resolutionId':
                return (value: number, value2: number, sourceRow: SingleRouteItem) =>
                {
                    return GridHelpersFunctions.enumBitwiseAndCompare(value, value2) ||
                        GridHelpersFunctions.enumBitwiseAndCompare(value2, value);
                };
        }

        return undefined;
    }
}