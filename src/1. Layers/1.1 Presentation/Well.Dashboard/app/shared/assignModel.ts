import {Branch} from './branch/branch';
import {IUser} from './iuser';

export class AssignModel
{
    public assigned: string;
    public branch: Branch;
    public jobIds: number[];
    public isReadOnlyUser: boolean;
    public objectSource: any;

    constructor(assigned: string, branch: Branch, jobIds: number[], readOnlyUser: boolean, objectSource: any)
    {
        this.assigned = assigned;
        this.branch = branch;
        this.jobIds = jobIds;
        this.isReadOnlyUser = readOnlyUser;
        this.objectSource = objectSource;
    }
}

export class AssignModalResult
{
    public assigned: boolean;
    public source: any;
    public newUser: IUser;
}