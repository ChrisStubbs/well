import {Branch} from '../shared/branch/branch';

export class SeasonalDate {
    public id: number;
    public branchName: string;
    public description: string;
    public fromDate: string;
    public toDate: string;
    public branches: Branch[];
}