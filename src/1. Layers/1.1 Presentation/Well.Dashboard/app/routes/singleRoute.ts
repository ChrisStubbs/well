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

export interface SingleRouteItem
{
    jobId: number;
    stopId: number;
    stop: string;
    stopStatus: string;
    stopExceptions: number;
    stopClean: number;
    tba: number;
    stopAssignee: string;
    resolution: string;
    invoice: string;
    jobType: string;
    cod: string;
    pod: boolean;
    exceptions: number;
    clean: number;
    credit?: number;
    assignee: string;
    selected?: boolean;
    jobStatusDescription: string;
    jobStatus: number;
}