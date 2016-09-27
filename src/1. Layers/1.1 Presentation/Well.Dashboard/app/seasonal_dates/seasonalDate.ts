import {Branch} from '../shared/branch/branch';

export class SeasonalDate {
    id: number;
    branchName: string;
    description: string;
    fromDate: string;
    toDate: string;
    branches: Branch[];
}