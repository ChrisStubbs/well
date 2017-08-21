import { DatePipe } from '@angular/common';

export class Route
{
    private datePipe: DatePipe;

    constructor()
    {
        this.datePipe = new DatePipe('en-Gb');
    }

    public id: number;
    public branchId: number;
    public branch: string;
    public routeNumber: string;
    public routeDate: Date;
    public get dateFormatted(): string
    {
        return this.datePipe.transform(this.routeDate, 'yyyy-MM-dd');
    }
    public stopCount: number;
    public routeStatusId: number;
    public routeStatus: string;
    public exceptionCount: number;
    public cleanCount: number;
    public driverName: string;
    public assignee: string;
    public jobIds: number[];
    public jobIssueType: number;
}
