import * as _ from 'lodash';

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

export class SingleRouteItem
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
}