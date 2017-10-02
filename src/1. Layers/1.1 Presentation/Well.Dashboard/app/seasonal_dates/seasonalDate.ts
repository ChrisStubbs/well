import {Branch} from '../shared/branch/branch';

export class SeasonalDate {
    public id: number;
    public branchName: string;
    public description: string;
    public fromDate: Date;
    public toDate: Date;
    public branches: Branch[];
}