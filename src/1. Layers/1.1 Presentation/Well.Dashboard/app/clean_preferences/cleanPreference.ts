import {Branch} from '../shared/branch/branch';

export class CleanPreference {
    public id: number;
    public days: number;
    public branchName: string;
    public branches: Branch[];
}