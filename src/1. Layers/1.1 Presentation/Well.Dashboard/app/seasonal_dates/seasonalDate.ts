import {Branch} from '../shared/branch/branch';

export class SeasonalDate {
    id: number;
    branchName: string;
    description: string;
    from: string;
    to: string;
    branches: Branch[]
}