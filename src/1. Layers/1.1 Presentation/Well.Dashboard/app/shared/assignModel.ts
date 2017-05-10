import {Branch} from './branch/branch';

export class AssignModel
{
    public assigned: string;
    public branch: Branch;
    public jobIds: number[];
    public isReadOnlyUser: boolean;
    public assignedTo: string;

    constructor(assigned: string, branch: Branch, jobIds: number[], readOnlyUser: boolean)
    {
        this.assigned = assigned;
        this.branch = branch;
        this.jobIds = jobIds;
        this.isReadOnlyUser = readOnlyUser;
    }
}