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
    public cod: string;
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

}