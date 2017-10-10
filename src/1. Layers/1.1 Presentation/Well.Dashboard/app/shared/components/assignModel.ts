import { Branch } from '../branch/branch';
import { IUser } from '../models/iuser';

export class AssignModel {
    public assigned: string;
    public branch: Branch;
    public jobIds: number[];
    public objectSource: any;
    public allocatePendingApprovalJobs: boolean;

    constructor(
        assigned: string,
        branch: Branch,
        jobIds: number[],
        objectSource: any,
        allocatePendingApprovalJobs: boolean) {
        this.assigned = assigned;
        this.branch = branch;
        this.jobIds = jobIds;
        this.objectSource = objectSource;
        this.allocatePendingApprovalJobs = allocatePendingApprovalJobs;
    }
}

export class AssignModalResult {
    public assigned: boolean;
    public source: any;
    public newUser: IUser;
}