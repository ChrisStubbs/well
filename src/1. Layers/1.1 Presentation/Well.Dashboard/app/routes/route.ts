import { AssignModel } from '../shared/assignModel';
import { Branch } from '../shared/branch/branch';

export class Route
{
    public branchId: number;
    public branch: string;
    public route: string;
    public routeDate: Date;
    public stopCount: number;
    public routeStatusId: number;
    public routeStatus: string;
    public exceptionCount: number;
    public hasExceptions: boolean;
    public driverName: string;
    public assignee: string;
    public jobIds: number[];
}