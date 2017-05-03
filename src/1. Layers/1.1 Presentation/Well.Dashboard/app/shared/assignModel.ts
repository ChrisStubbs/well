import {Branch} from './branch/branch';

export class AssignModel
{
    public assigned: string;
    public branch: Branch;
    public jobIds: number[];

    constructor(assigned: string, branch: Branch, jobIds: number[])
    {
        this.assigned = assigned;
        this.branch = branch;
        this.jobIds = jobIds;
    }
}