export class Route
{
    public id: number;
    public branchId: number;
    public branch: string;
    public routeNumber: string;
    public routeDate: Date;
    public stopCount: number;
    public routeStatusId: number;
    public routeStatus: string;
    public exceptionCount: number;
    public cleanCount: number;
    public hasExceptions: boolean;
    public hasClean: boolean;
    public driverName: string;
    public assignee: string;
    public jobIds: number[];
}
